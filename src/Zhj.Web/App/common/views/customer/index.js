(function () {
    appModule.controller('common.views.customer.index', [
        '$scope', '$uibModal', 'abp.services.app.card','abp.services.app.customer','appSession',
        function ($scope, $uibModal, cardService, customerService, appSession) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });
            vm.customer = { customerState: 1 };
            vm.filter = {
                name:'',card:'',mobile:'',point:''
            }
            vm.userState = [{ id: 1, text: '普通用户' },
                    { id: 2, text: '公司员工' }, { id: 3, text: '社区员工' }];

            vm.type = 1;
          
            vm.date = {
                rightopen: false,
                //left: new Date(),
                //right:new date(),
                inlineOptions: {
                    // customClass: getDayClass,
                    // minDate: new Date(),
                    showWeeks: true
                },
                dateOptions: {
                    //dateDisabled: disabled,
                    formatYear: 'yyyy',
                    maxDate: new Date(),
                    minDate: new Date(1900, 1, 1),
                    startingDay: 1
                },
                openright: function () {
                    vm.date.rightopen = !vm.date.rightopen;
                    // $scope.popup2.opened = !$scope.popup2.opened;
                }
            }

            vm.edit = function (item) {
                vm.customer = item;
                vm.customer.birthday = new Date(item.birthday);
                vm.type = 2;
            }
            vm.save = function () {
                customerService.insertCustomer(vm.customer).success(function () {
                    abp.notify.success(vm.customer.id > 0 ? '编辑成功' : '添加成功');
                    vm.customer = {};
                    vm.type = 1;
                    vm.init();
                })
            }
            vm.action = function (type, customer) {
                if (customer.cardId <= 0)
                {
                    abp.notify.info("未绑定会员卡");
                    return;
                }
                var modal = $uibModal.open({
                    templateUrl: '~/App/common/views/customer/modal.cshtml',
                    controller: 'common.views.customer.modal as vm',
                    backdrop: 'static',
                    resolve: {
                        model: function () { return { type: type, customer: customer } }
                    }
                });
                modal.result.then(function (response) {
                    vm.init();

                    if (type == 1) {//充值
                        var modal = $uibModal.open({
                            templateUrl: '~/App/common/views/customer/print.cshtml',
                            controller: 'common.views.customer.print as vm',
                            backdrop: 'static',
                            resolve: {
                                order: function () {
                                    var a = {
                                        customer: customer,
                                        needto: response.nums,
                                        user: appSession.user,
                                        time: moment().format('YYYY-MM-DD HH:mm')
                                    }
                                    return a;
                                }
                            }
                        });
                        modal.result.then(function (response) {
                            vm.card = '';
                            vm.step = 1;
                            vm.order = { orderState: 1 };
                        });
                    }
                });
              


            };
            vm.nobindcustomers = function () {
                var page = vm.table.pageIndex;
                var display = vm.table.pageSize;
                vm.loading = true;
                customerService.getNoBindCustomerList({
                    skipCount: (page - 1) * display,
                    maxResultCount: display,
                    name: vm.filter.name,
                    mobile: vm.filter.mobile,
                    card: vm.filter.card,
                    point: vm.filter.point
                }).success(function (result) {
                    vm.table.data = result.items;
                    vm.table.getPageList(result.totalCount);
                }).finally(function () {
                    vm.loading = false;
                });

            }

            vm.selectcard = function () {
                var modal = $uibModal.open({
                    templateUrl: '~/App/common/views/customer/cardmodal.cshtml',
                    controller: 'common.views.customer.cardmodal as vm',
                    backdrop: 'static',
                });
                modal.result.then(function (response) {
                    vm.customer.cardId = response.id;
                    vm.customer.outCard = response.outCard;
                });
            };
            vm.selectpoint = function () {
                var modal = $uibModal.open({
                    templateUrl: '~/App/common/views/customer/pointmodal.cshtml',
                    controller: 'common.views.customer.pointmodal as vm',
                    backdrop: 'static',
                  
                });
                modal.result.then(function (response) {
                    vm.customer.pointId = response.id;
                    vm.customer.pointName = response.pointname;
                });
            }


            vm.delete = function (item) {
                abp.message.confirm(
                   '确定要删除' + item.customerName,
                    function (isConfirmed) {
                        if (isConfirmed) {
                            customerService.deleteCustomer({
                                id: item.id
                            }).success(function () {
                                vm.init();
                                abp.notify.success('删除成功');
                            });
                        }
                    }
                );
            };


            vm.cancel = function () {
                vm.customer = {};
                vm.type = 1;
            }
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
                 
                    if (vm.table.pageList.length>0&&vm.table.pageList.indexOf(vm.table.pageIndex)>-1) {
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
                customerService.getCustomerList({
                    skipCount: (page - 1) * display,
                    maxResultCount: display,
                    name: vm.filter.name,
                    mobile: vm.filter.mobile,
                    card: vm.filter.card,
                    point: vm.filter.point
                }).success(function (result) {
                    vm.table.data = result.items;
                    vm.table.getPageList(result.totalCount);
                }).finally(function () {
                    vm.loading = false;
                });

            }
            vm.init();

         
        }
    ]);
})();