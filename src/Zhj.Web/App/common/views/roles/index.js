(function () {

    appModule.controller('common.views.roles.index', [
        '$scope', '$uibModal', '$templateCache', 'abp.services.app.role',
        function ($scope, $uibModal, $templateCache, roleService) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.loading = false;
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
                    vm.init();
                    vm.table.isActivePage(page);
                }
            }
            vm.init = function () {
                vm.loading = true;

                roleService.getRoles({}).success(function (data) {
                    vm.table.data = data.items;
                }).finally(function () {
                    vm.loading = false;
                });
            }
          
            vm.editRole = function (role) {
                openCreateOrEditRoleModal(role.id);
            };

            vm.deleteRole = function (role) {
                abp.message.confirm(
                    app.localize('RoleDeleteWarningMessage', role.displayName),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            roleService.deleteRole({
                                id: role.id
                            }).success(function () {
                                vm.getRoles();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                        }
                    }
                );
            };

            vm.createRole = function () {
                openCreateOrEditRoleModal(null);
            };

            function openCreateOrEditRoleModal(roleId) {
                var modalInstance = $uibModal.open({
                    templateUrl: '~/App/common/views/roles/createOrEditModal.cshtml',
                    controller: 'common.views.roles.createOrEditModal as vm',
                    backdrop: 'static',
                    resolve: {
                        roleId: function () {
                            return roleId;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    vm.getRoles();
                });
            }

            vm.init();
        }
    ]);
})();