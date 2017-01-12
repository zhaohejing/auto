
(function () {
    appModule.controller('common.views.order.index', [
        '$scope', '$uibModal', 'abp.services.app.card','$filter','userList',
    function ($scope, $uibModal, cardService, $filter, userList) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });
            vm.step = 1;
            vm.card;
            vm.customer;
            vm.cardlist = [];
            vm.nopaylist = [];
            vm.payState = 2;
    
            vm.order = {orderState:1};
            vm.datemodel = [];
            vm.init = function () {
                cardService.getOrderCardUser({
                    filter: ''
                }).success(function (result) {
                    vm.cardlist =  result.items;
                });
            }
    


            vm.getOrders = function (id,time) {
                cardService
                 .getUserOrders({customerId:id,time:null })
                 .success(function (result) {
                     if (result) {
                         vm.datemodel = result.items;
                     } else {
                         vm.datemodel = [];
                     }
                 });
            }

            vm.init();
       

            $scope.cardAdded = function (tag) {
                if (vm.order.card || vm.order.name) {
                    return;
                }
                vm.order.customerId = tag.id;
                vm.order.integral = tag.integral;
                vm.order.card = tag.outCard;
                vm.order.name = tag.customerName;
                vm.order.phone = tag.customerPhone;
                vm.order.balance = tag.balance;
                vm.order.pointId = tag.pointId;
                vm.order.pointName = tag.pointName;
            }
            $scope.cardRemoved = function (tag) {
                if (tag.inCard!=vm.order.card) {
                    return;
                }
                vm.order = { orderState: 1 };
            }

            $scope.loadcard = function (v) {
                return $filter('filter')(userList.list, { outCard: v });
            }

            $scope.loadcustomer = function (v) {
                return $filter('filter')(userList.list, { customerName: v });

            }
            


            vm.currentDate;
            vm.clickdate = function () {

                var modal = $uibModal.open({
                    templateUrl: '~/App/common/views/order/modal.cshtml',
                    controller: 'common.views.order.modal as vm',
                    backdrop: 'static',
                    resolve: {
                        order: function () {
                            vm.order.ordertime = vm.currentDate;
                            return vm.order;
                        }
                    }
                });
                modal.result.then(function (response) {
                    if (response) {
                        vm.getOrders(vm.order.customerId, moment());
                    } else {
                        vm.step = 1;
                        vm.order = {};
                    }
                });
            }
            vm.paying = false;
            vm.payfor = function () {
                vm.paying = true;
                var mo = {
                    customerId: vm.order.customerId,
                    inCard: vm.order.card,
                    payState: vm.payState,
                    totalPrice: vm.totalprice,
                    time: moment(),
                    orders: vm.nopaylist
                }
                if (!mo.customerId||!mo.orders) {
                    return;
                }
                cardService
                .payForOrders(mo)
                .success(function (res) {
                    if (res.error=="成功") {
                        abp.notify.success('支付成功');
                        var modal = $uibModal.open({
                            templateUrl: '~/App/common/views/order/print.cshtml',
                            controller: 'common.views.order.print as vm',
                            backdrop: 'static',
                            resolve: {
                                order: function () {
                                    var a = {
                                        order: vm.order,
                                        dishes: vm.nopaylist,
                                        totalprice: vm.totalprice,
                                        payState: vm.payState
                                    }
                                    return a;
                                }
                            }
                        });
                        modal.result.then(function (response) {
                            vm.card = '';
                            vm.step = 1;
                            userList.reload();
                            vm.order = { orderState: 1 };
                        });
                    } else {
                        abp.message.error("", res.error);
                        vm.card = '';
                        vm.step = 1;
                        vm.order = { orderState: 1 };
                    }
                 
                }).finally(function () {
                    vm.paying = false;
                });
              
             
            }
            vm.totalprice = 0;
            vm.prev = function () {
                if (vm.step<=1) {
                    return;
                }
                vm.step--;
            }
            vm.next = function () {
                if (vm.order.customerId) {
                    vm.getOrders(vm.order.customerId,null);
                }

                if (vm.step>=3) {
                    return;
                }
                vm.step++;
            }
            vm.getOrder = function () {
                if (vm.step >= 3) {
                    return;
                }
                if (!vm.order) {
                    return;
                }
                cardService
                 .getUserNoPayOrders({ customerId: vm.order.customerId, time: null })
                 .success(function (result) {
                     if (result) {
                         vm.totalprice = 0.00;
                         vm.nopaylist = result.items;
                         angular.forEach(vm.nopaylist, function (v, i) {
                             vm.totalprice += v.dishCost * (1.0* v.dishNumber);
                         })
                         vm.totalprice = vm.totalprice.toFixed(2);
                         vm.step++;
                     } else {
                         vm.nopaylist = [];
                     }
                 });
             
            }

            //start from here...
            //点击收回事件
            $(document).mouseup(function (e) {
                e.stopPropagation();

                var _con = $('#calendar');   // 设置目标区域
                if (!_con.is(e.target) && _con.has(e.target).length === 0) {
                    return;
                } else {
                    if (e.target.className == 'day') {
                        vm.currentDate = e.target.attributes["time"].nodeValue;

                    } else if (e.target.className == 'day-number') {
                        if (e.target.parentElement.attributes["time"] != undefined) {
                            vm.currentDate = e.target.parentElement.attributes["time"].nodeValue;

                        }
                    }
                }

            });

        }
    ]);
})();