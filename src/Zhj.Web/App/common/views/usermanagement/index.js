(function () {
    appModule.controller('common.views.usermanagement.index', [
        '$scope', '$uibModal', 'abp.services.app.user',
        function ($scope, $uibModal, userService) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });
            vm.isedit = false;
            vm.usertype = [{ type: 0, typeName: '未设置' }, { type: 1, typeName: '系统管理' },
                            { type: 2, typeName: '部门经理' }, { type: 3, typeName: '普通用户' }];

            vm.userstype = [ { type: 1, typeName: '系统管理' },
                           { type: 2, typeName: '部门经理' }, { type: 3, typeName: '普通用户' }];
        
            vm.permissions = {
                create: abp.auth.hasPermission('Pages.OrganizationUser.UserManagement.Create'),
                edit: abp.auth.hasPermission('Pages.OrganizationUser.UserManagement.Edit'),
                'delete': abp.auth.hasPermission('Pages.OrganizationUser.UserManagement.Delete'),
            };

            vm.user = {};
            vm.filter = "";

            vm.type = 1;
            vm.isorg = true;
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
                    vm.getusers();
                    vm.table.isActivePage(page);
                }
            }



            vm.getusers = function () {
                var page = vm.table.pageIndex;
                var display = vm.table.pageSize;

                vm.loading = true;
                userService.getUserList({
                    skipCount: (page - 1) * display,
                    maxResultCount: display,
                    filter: vm.filter,
                    Sorting: 'creationTime desc'

                }).success(function (result) {
                    vm.table.data = result.items;
                    vm.table.getPageList(result.totalCount);

                }).finally(function () {

                    vm.loading = false;
                });
            };
            //vm.getusers = function (page) {
            //    var display = vm.table.pageSize;

            //    vm.loading = true;
            //    userService.getUserList({
            //        skipCount: (page - 1) * display,
            //        maxResultCount: display,
            //        filter: vm.filter,
            //        Sorting: 'creationTime desc'

            //    }).success(function (result) {
            //        vm.table.data = result.items;
            //        vm.table.getPageList(result.totalCount);

            //    }).finally(function () {

            //        vm.loading = false;
            //    });
            //};

            vm.edit = function (item) {
                vm.user = angular.copy(item);
                vm.user.password = "";
                vm.user.passwordRepeat = "";
                vm.isedit = true
                vm.type = 2;
            };
            vm.deletethis = function (item) {
                userService.deleteUser({
                    id: item.id

                }).success(function (result) {
                    abp.notify.info('删除用户成功');
                    vm.getusers();
                }).finally(function () {

                    vm.loading = false;
                });
            };
            vm.cancel = function () {
                vm.type = 1;
            };

            vm.save = function (type, form) {
                vm.user.surName = vm.user.name;
                if (!vm.user.id && !vm.user.password) {
                    abp.notify.info("密码不可为空");
                    return;
                }
                if (vm.user.password != vm.user.passwordRepeat)
                {
                    abp.notify.info("密码输入不一致");
                    return;
                }


                vm.saving = true;
                userService.createOrUpdateUser({
                    user: vm.user,
                    assignedRoleNames: [vm.user.roleName],
                    sendActivationEmail: false
                }).success(function () {
                    abp.notify.info(vm.user.id > 0 ? "编辑用户成功" : '创建用户成功');
                    vm.user = {};
                    vm.getusers();
                }).finally(function () {
                    vm.saving = false;
                });
            };
            vm.showorg = function () {
                var modal = $uibModal.open({
                    templateUrl: '~/App/common/views/usermanagement/selectDepartment.cshtml',
                    controller: 'common.views.usermanagement.selectDepartment as vm',
                    backdrop: 'static',
                    resolve: {

                    }
                });
                modal.result.then(function (model) {
                    if (model != null) {
                        vm.user.departmentId = model.id;
                        vm.user.departmentName = model.name;

                    }

                })
            };

            vm.adduser = function () {
                vm.user = {};
            };

            vm.getusers();

        }
    ]);
})();