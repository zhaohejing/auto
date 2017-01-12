(function () {
    appModule.controller('common.views.layout.header', [
        '$rootScope', '$scope', '$uibModal', 'appSession',
        function ($rootScope, $scope, $uibModal, appSession) {
            var vm = this;

            $scope.$on('$includeContentLoaded', function () {
                Layout.initHeader(); // init header
            });

            vm.languages = abp.localization.languages;
            vm.currentLanguage = abp.localization.currentLanguage;
            vm.isImpersonatedLogin = abp.session.impersonatorUserId;
            vm.notifications = [];
            vm.unreadNotificationCount = 0;
            vm.recentlyUsedLinkedUsers = [];

            vm.getShownUserName = function () {
                if (!abp.multiTenancy.isEnabled) {
                    return appSession.user.userName;
                } else {
                    if (appSession.tenant) {
                        return appSession.tenant.tenancyName + '\\' + appSession.user.userName;
                    } else {
                        return '.\\' + appSession.user.userName;
                    }
                }
            };    
            vm.changePassword = function () {
                $uibModal.open({
                    templateUrl: '~/App/common/views/profile/changePassword.cshtml',
                    controller: 'common.views.profile.changePassword as vm',
                    backdrop: 'static'
                });
            };
        }
    ]);
})();