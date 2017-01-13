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

            vm.type = 1;
            vm.isorg = true;
       
            //页面属性
            vm.jTableInfo = {
                data: [],               //数据集
                filter: "",
                pageConfig: {           //分页配置
                    currentPage: 1,
                    itemsPerPage: 10,
                    totalItems: 0,
                    onChange: function () {
                        vm.getusers();
                    }
                }
              
            };

            //获取用户数据集，并且添加配置项
            vm.getusers = function () {
                vm.loading = true;
                var page = vm.jTableInfo.pageConfig.currentPage;
                var display = vm.jTableInfo.pageConfig.itemsPerPage;

                userService.getUserList({
                    skipCount: (page - 1) * display,
                    maxResultCount: display,
                    filter: vm.jTableInfo.filter,
                    Sorting: 'creationTime desc'
                }).success(function (result) {
                    vm.jTableInfo.pageConfig.totalItems = result.totalCount;
                    vm.jTableInfo.data = result.items;
                }).finally(function () {
                    vm.loading = false;
                });
            };



     


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