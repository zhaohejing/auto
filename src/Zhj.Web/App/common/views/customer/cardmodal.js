(function () {
    appModule.controller('common.views.customer.cardmodal', [
        '$scope', '$uibModalInstance', 'abp.services.app.card',
        function ($scope, $uibModalInstance,cardService) {
            var vm = this;
            vm.filter = "";
            vm.saving = false;
            vm.check;
            vm.select = function (mo) {
                vm.check = mo;
            }
            vm.save = function () {
                if (!vm.check) {
                    abp.notify.warn("请选择卡号");
                    return;
                }
                $uibModalInstance.close(vm.check);
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
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
                cardService.getUsedCardList({
                    skipCount: (page - 1) * display,
                    maxResultCount: display, outNum: vm.filter
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