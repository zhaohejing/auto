(function () {
    appModule.factory('appSession', [
            function () {

                var _session = {
                    user: null,
                    tenant: null
                };

                abp.services.app.session.getCurrentLoginInformations({ async: false }).done(function (result) {
                    _session.user = result.user;
                    _session.tenant = result.tenant;
                });

                return _session;
            }
    ]);


})();
(function () {
    appModule.factory('userList', [
            function () {

                var _user = {
                    list:null
                };

                abp.services.app.card.getOrderCardUser({ async: false }).done(function (result) {
                    _user.list = result.items;
                });
                _user.reload = function () {
                    abp.services.app.card.getOrderCardUser({ async: false }).done(function (result) {
                        _user.list = result.items;
                    });
                }


                return _user;
            }
    ]);


})();