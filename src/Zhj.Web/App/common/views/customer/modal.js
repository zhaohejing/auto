(function () {
    appModule.controller('common.views.customer.modal', [
        '$scope', '$uibModalInstance', 'model', 'abp.services.app.card',
        function ($scope, $uibModalInstance, model, cardService) {
            var vm = this;
            vm.type = model.type;
            vm.customer = model.customer;
            if (vm.type==3) {
                vm.needto=vm.customer.balance;
            }
            vm.saving = false;

            vm.save = function () {
                cardService.actionCard({ type: vm.type, cardId: vm.customer.cardId, nums: vm.needto })
                    .success(function () {
                        abp.notify.success('操作成功');
                        $uibModalInstance.close({ type: vm.type, cardId: vm.customer.cardId, nums: vm.needto });
                    });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };
        }
    ]);
})();