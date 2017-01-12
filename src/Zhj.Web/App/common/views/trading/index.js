(function () {
    appModule.controller('common.views.trading.index', [
        '$scope', '$uibModal', '$state', 'abp.services.app.customer', 'abp.services.app.card',
        function ($scope, $uibModal, $state,customerService,cardService) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });
            vm.filter = { skipCount: 0, maxResultCount: 10, orderState: 0, payState: 0 };
            vm.orderState = [{id:0,text:'全部订单'},{id:1,text:'新订单'},{id:2,text:'支付成功'},{id:3,text:'已交货'}
            ,{id:4,text:'失效订单'}
            ,{id:5,text:'用户取消'}
            ,{id:6,text:'发货中'}
            ,{id:7,text:'异常订单'}
            ,{id:8,text:'已退货'}
            ]
            vm.userState = [{ id: 1, text: '普通用户' },
                     { id: 2, text: '公司员工' }, { id: 3, text: '社区员工' }];

            vm.date = {
                leftopen: false,
                rightopen: false,
                //left: new Date(),
                //right:new date(),
                inlineOptions: {
                    // customClass: getDayClass,
                    // minDate: new Date(),
                    showWeeks: false
                },
                dateOptions: {
                    //dateDisabled: disabled,
                    formatYear: 'yyyy',
                    maxDate: new Date(2020, 1, 1),
                    minDate:  new Date(1900, 1, 1),
                    startingDay: 1
                },
                openleft: function () {
                    vm.date.leftopen = !vm.date.leftopen;
                   // $scope.popup2.opened = !$scope.popup2.opened;
                },
                openright: function () {
                    vm.date.rightopen = !vm.date.rightopen;
                    // $scope.popup2.opened = !$scope.popup2.opened;
                }
            }

            vm.payState = [{id:0,text:'全部渠道'},{id:1,text:'现金'},{id:2,text:'会员卡'}
                , { id: 3, text: '支付宝' }, { id: 4, text: '微信' }, { id: 5, text: '积分' }, { id: 6, text: '兑换券' },
                { id: 7, text: '退订款' }]

            vm.order = function () {
                $state.go('order');
            }
            ///权限
            vm.permissions = {
                'backoff': abp.auth.hasPermission('Pages.TradingInfo.BeBackOff'),
            
            };

            vm.table = {
                data: [],
                pageSize: 10,
                pageIndex: 1,
                totalCount: 0,
                pages: 0,
                pageList: [],
                isActivePage: function (page) {
                    return vm.table.pageIndex == page;
                },
                //首页
                first: function () {
                    vm.table.selectPage(1);
                }, //尾页
                last: function () {
                    vm.table.selectPage(vm.table.pages);
                },
                //上一页
                prev: function () {
                    vm.table.selectPage(vm.table.pageIndex - 1);
                },
                //下一页
                next: function () {
                    vm.table.selectPage(vm.table.pageIndex + 1);
                },
                getPageList: function (count) {
                    if (vm.table.pageList.length > 0 && vm.table.pageList.indexOf(vm.table.pageIndex) > -1) {
                        return;
                    } else {
                        vm.table.pageList = [];
                    }
                    vm.table.totalCount = count;
                    vm.table.pages = Math.ceil(count / vm.table.pageSize); //分页数
                    var aa = vm.table.pages > 5 ? 5 : vm.table.pages;
                    //分页要repeat的数组
                    for (var i = 0; i < aa; i++) {
                        vm.table.pageList.push(i + 1);
                    }
                },

                selectPage: function (page) {
                    //不能小于1大于最大
                    if (page < 1 || page > vm.table.pages) return;
                    //最多显示分页数5
                    if (page > 2) {
                        //因为只显示5个页数，大于2页开始分页转换
                        var newpageList = [];
                        for (var i = (page - 3) ; i < ((page + 2) >
                            vm.table.totalCount ? vm.table.totalCount : (page + 2)) ; i++) {
                            newpageList.push(i + 1);
                        }
                        vm.table.pageList = newpageList;
                    }
                    vm.table.pageIndex = page;
                    vm.init();
                    vm.table.isActivePage(page);
                }
            }
            vm.init = function () {
                var page = vm.table.pageIndex;
                var display = vm.table.pageSize;
                vm.loading = true;
                vm.filter.skipCount = (page - 1) * display;
                vm.filter.maxResultCount = display;

                customerService.getUserOrders(vm.filter).success(function (result) {
                    vm.table.data = result.items;
                    vm.table.getPageList(result.totalCount);
                }).finally(function () {
                    vm.loading = false;
                });

            }

            vm.export = function () {
                customerService.exportOrderInfo(vm.filter)
                 .success(function (result) {
                     app.downloadTempFile(result);
                 });

            }

            vm.backoff = function (order) {
                cardService.backOffOrder({id:order.id})
              .success(function (result) {
                  vm.init();
              });
            }
            vm.bebackoff = function (order) {
                cardService.beBackOffOrder({ id: order.id })
              .success(function (result) {
                  vm.init();
              });
            }
            vm.init();



        }
    ]);
})();