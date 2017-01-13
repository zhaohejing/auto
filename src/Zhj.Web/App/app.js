/* 'app' MODULE DEFINITION */
var appModule = angular.module("app", [
    "ui.router",
    "ui.bootstrap",
    'ui.utils',
    "ui.jq",
    'ui.grid',
    'ui.grid.pagination',
    "oc.lazyLoad",
    "ngSanitize",
    'angularFileUpload',
    'daterangepicker',
    'angularMoment',
    'frapontillo.bootstrap-switch',
    'abp', 'ngTagsInput',
    'bsTable', 'objPagination'
]);

/* LAZY LOAD CONFIG */

/* This application does not define any lazy-load yet but you can use $ocLazyLoad to define and lazy-load js/css files.
 * This code configures $ocLazyLoad plug-in for this application.
 * See it's documents for more information: https://github.com/ocombe/ocLazyLoad
 */
appModule.config(['$ocLazyLoadProvider', function ($ocLazyLoadProvider) {
    $ocLazyLoadProvider.config({
        cssFilesInsertBefore: 'ng_load_plugins_before', // load the css files before a LINK element with this ID.
        debug: false,
        events: true,
        modules: []
    });
}]);

/* THEME SETTINGS */
App.setAssetsPath(abp.appPath + 'metronic/assets/');
appModule.factory('settings', ['$rootScope', function ($rootScope) {
    var settings = {
        layout: {
            pageSidebarClosed: false, // sidebar menu state
            pageContentWhite: true, // set page content layout
            pageBodySolid: false, // solid body color state
            pageAutoScrollOnLoad: 1000 // auto scroll to top on page load
        },
        layoutImgPath: App.getAssetsPath() + 'admin/layout4/img/',
        layoutCssPath: App.getAssetsPath() + 'admin/layout4/css/',
        assetsPath: abp.appPath + 'metronic/assets',
        globalPath: abp.appPath + 'metronic/assets/global',
        layoutPath: abp.appPath + 'metronic/assets/layouts/layout4'
    };

    $rootScope.settings = settings;

    return settings;
}]);

/* ROUTE DEFINITIONS */

appModule.config([
    '$stateProvider', '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider) {

        if (abp.auth.hasPermission('Pages.TradingInfo.PhoneBook')) {
            $urlRouterProvider.otherwise("/phonebook");
            //添加隐藏页面的方法 跳转 传参等等
            $stateProvider.state('phonebook', {
                url: '/phonebook', data: { pageTitle: "电话预定" },
                templateUrl: '~/App/common/views/trading/index.cshtml',
                menu: 'PhoneBook'
            })
        }
        //客户信息
        if (abp.auth.hasPermission('Pages.CustomerManagement.CustomerInfo')) {
            $stateProvider.state('customerinfo', {
                url: '/customerinfo',
                templateUrl: '~/App/common/views/customer/index.cshtml',
                menu: 'CustomerInfo'
            });
        }
        //会员卡信息
        if (abp.auth.hasPermission('Pages.CustomerManagement.CustomerCardInfo')) {
            $stateProvider.state('customercardinfo', {
                url: '/customercardinfo',
                templateUrl: '~/App/common/views/customercard/index.cshtml',
                menu: 'CustomerCardInfo'
            });
        }
        //预定
        $stateProvider.state('order', {
            url: '/order',
            templateUrl: '~/App/common/views/order/index.cshtml',
            menu: ''
        });
      
        //COMMON routes
        
        if (abp.auth.hasPermission('Pages.ReportManagement.PointOrder')) {
            $stateProvider.state('pointorder', {
                url: '/pointorder',
                templateUrl: '~/App/common/views/report/pointOrder.cshtml',
                menu: 'PointOrder'
            });
        }
        //------------------------------------------------
   
         if (abp.auth.hasPermission('Pages.ReportManagement.PrepaidRecords')) {
             $stateProvider.state('prepaidrecords', {
                url: '/prepaidrecords',
                templateUrl: '~/App/common/views/report/prepaid.cshtml',
                menu: 'PrepaidRecords'
            });
        }
         if (abp.auth.hasPermission('Pages.ReportManagement.PointOrderRecords')) {
             $stateProvider.state('pointorderrecords', {
                 url: '/pointorderrecords',
                templateUrl: '~/App/common/views/report/duerecords.cshtml',
                menu: 'PointOrderRecords'
            });
        }


        if (abp.auth.hasPermission('Pages.OrganizationUser.RoleManagement')) {
            $stateProvider.state('role', {
                url: '/role',
                templateUrl: '~/App/common/views/roles/index.cshtml',
                menu: 'Role'
            });
        }


        if (abp.auth.hasPermission('Pages.ReportManagement.DayDue')) {
            $stateProvider.state('daydue', {
                url: '/daydue',
                templateUrl: '~/App/common/views/report/daydue.cshtml',
                menu: 'DayDue'
            });
        }
        if (abp.auth.hasPermission('Pages.ReportManagement.MemberConsumptionRecord')) {
            $stateProvider.state('memberconsumptionrecord', {
                url: '/memberconsumptionrecord',
                templateUrl: '~/App/common/views/report/customOrder.cshtml',
                menu: 'MemberConsumptionRecord'
            });
        }
        if (abp.auth.hasPermission('Pages.OrganizationUser.UserManagement')) {
            $stateProvider.state('usermanagement', {
                url: '/usermanagement',
                templateUrl: '~/App/common/views/usermanagement/index.cshtml',
                menu: 'UserManagement'
            });
        }
    }
]);

appModule.run(["$rootScope", "settings", "$state", 'i18nService', function ($rootScope, settings, $state, i18nService) {
    $rootScope.$state = $state;
    $rootScope.$settings = settings;

    //Set Ui-Grid language
    if (i18nService.get(abp.localization.currentCulture.name)) {
        i18nService.setCurrentLang(abp.localization.currentCulture.name);
    } else {
        i18nService.setCurrentLang("en");
    }

    $rootScope.safeApply = function (fn) {
        var phase = this.$root.$$phase;
        if (phase == '$apply' || phase == '$digest') {
            if (fn && (typeof (fn) === 'function')) {
                fn();
            }
        } else {
            this.$apply(fn);
        }
    };
}]);