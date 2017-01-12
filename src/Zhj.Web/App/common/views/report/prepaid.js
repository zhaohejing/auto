(function () {
    appModule.controller('common.views.report.prepaid', [
        '$scope', '$uibModal', 'abp.services.app.order',
        function ($scope, $uibModal, orderService) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });
            vm.date = {
                leftopen: false,
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
                    maxDate: new Date(2020, 5, 22),
                    minDate: new Date(1900, 1, 1),
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
            vm.filter = "";
            vm.userState = [{ id: 1, text: '普通用户' },
              { id: 2, text: '公司员工' }, { id: 3, text: '社区员工' }];
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
                //上一页
                prev: function () {
                    vm.table.selectPage(vm.table.pageIndex - 1);
                },
                //下一页
                next: function () {
                    vm.table.selectPage(vm.table.pageIndex + 1);
                },
                getPageList: function (count) {
                    if (vm.table.pageList.length > 0) {
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
                    vm.getorders();
                    vm.table.isActivePage(page);
                }
            }

            vm.getorders = function () {

                var page = vm.table.pageIndex;
                var display = vm.table.pageSize;

                vm.loading = true;
                orderService.getPointOrders({
                    skipCount: (page - 1) * display,
                    maxResultCount: display,
                    filter: vm.filter,
                    searchStartTime: vm.startTime,
                    searchEndTime: vm.endTime
                }).success(function (result) {
                    vm.table.data = result.items;
                    vm.table.getPageList(result.totalCount);

                }).finally(function () {
                    vm.loading = false;
                });
            }
            vm.export = function () {
                var parms = { filter: vm.filter, searchStartTime: vm.startTime, searchEndTime: vm.endTime };
                orderService.exportPointOrders(parms)
                   .success(function (result) {
                       app.downloadTempFile(result);
                   });
            }

            vm.getorders();

        }
    ]);
})();