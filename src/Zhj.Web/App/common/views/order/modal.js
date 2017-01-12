(function () {
    appModule.controller('common.views.order.modal', [
        '$scope', '$uibModalInstance', "order", 'abp.services.app.card', '$filter',
        function ($scope, $uibModalInstance, order, cardService, $filter) {
            var vm = this;
            vm.order = order;
            vm.saving = false;

            vm.save = function () {
                if (vm.order.ordertime==null|| vm.order.ordertime==undefined||vm.order.ordertime<=moment()) {
                    abp.notify.warn('订餐时间错误');
                    return;
                }

                cardService
                 .insertOrder(vm.order)
                 .success(function (result) {
                     abp.notify.success(result ? '预定成功' : '预定失败');
                     vm.order.price = '';
                     vm.order.number = '';
                     $uibModalInstance.close(result);
                 });
            };
            vm.dish;
            vm.dishlist;

            vm.init = function () {
                vm.dishlist = [];
                cardService.getMenuList({
                    pointId: vm.order.pointId,
                    date: moment().format('YYYY-MM-DD')
                }).success(function (obj) {
                    angular.forEach(obj.result, function (v, i) {
                        var m =  {
                            id: v.product_id,
                            dish: v.product_name,
                            inCard:encodeURIComponent( v.product_name),
                            price: (v.price * 1.00 / 100).toFixed(2)
                        }
                        vm.dishlist.push(m);
                    });
                    //  vm.dishlist = obj.result;
                });
            }
            vm.init();

            $scope.tagAdded = function (tag) {
                if (vm.order.price) {
                    return;
                }
                vm.order.dishId = tag.id;
                vm.order.dish =tag.dish;
                vm.order.price = tag.price;
            }

            $scope.tagRemoved = function (tag) {
                if (tag.dish != vm.order.dish) {
                    return;
                }
                vm.order.dishId = null;
                vm.order.dish = null;
                vm.order.price = null;
            }
            $scope.loadTags = function (v) {
                return $filter('filter')(vm.dishlist, { inCard: encodeURIComponent(v) });
            }

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };
        }
    ]);
})();