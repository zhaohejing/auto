(function () {
    appModule.controller('common.views.customer.print', [
        '$scope', '$uibModalInstance', "order",
        function ($scope, $uibModalInstance, order, cardService) {
            var vm = this;
            vm.order = order;
            vm.saving = false;
            vm.save = function () {
                vm.print();
                $uibModalInstance.close();
            };

            vm.cancel = function () {
                $uibModalInstance.close();
            };
            vm.print = function () {
                $("#print").jqprint();
            };
        }
    ]);
})();