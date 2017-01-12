(function () {
    appModule.controller('common.views.customer.pointmodal', [
        '$scope', '$uibModalInstance', 'abp.services.app.card',
        function ($scope, $uibModalInstance, cardService) {
            var vm = this;

            vm.saving = false;
            vm.pointList;
            vm.check;
            vm.select = function (mo) {
                vm.check = mo;
            }

            vm.save = function () {
                if (!vm.check) {
                    return;
                }
                $uibModalInstance.close(vm.check);
            };
            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };
            function init() {
                cardService.getPointList().success(function (res) {
                    vm.pointList = res.result;
                });
            }

            init();
        }
    ]);
})();