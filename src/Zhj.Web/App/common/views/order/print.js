(function () {
    appModule.controller('common.views.order.print', [
        '$scope', '$uibModalInstance', "order",
        function ($scope, $uibModalInstance, order, cardService) {
            var vm = this;
            vm.order = order;
            vm.saving = false;
            vm.time = moment().format('YYYY-MM-DD');
            vm.save = function () {
                vm.print();
                $uibModalInstance.close(vm.order);
            };

            vm.cancel = function () {
                $uibModalInstance.close(vm.order);
            };
            vm.print = function () {
                $("#print").jqprint();
            };
        }
    ]);
})();