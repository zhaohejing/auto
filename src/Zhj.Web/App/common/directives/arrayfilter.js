//数组过滤
(function () {
    'use strict';
    appModule.filter('arrayfilter', function () {
        return function (array, index) {
            for (var i = 0; i < array.length; i++) {
                if (array[i].typeId == index) {
                    return array[i].typeName;
                }
            }
        };
    });
})();
(function () {
    appModule.filter('encodeURIComponent', function ($window) {
        return $window.encodeURIComponent;
    });
})();
(function () {
    'use strict';
    appModule.filter('enumFilter', function () {
        return function (index, array) {
            for (var i = 0; i < array.length; i++) {
                if (array[i].id == index) {
                    return array[i].text;
                }
            }
        };
    });
})();

//时间格式修改
(function () {
    'use strict';
    appModule.filter('timefilter', function () {
        return function (date) {
            var temp = date.replace(/-/gi, "/").replace('T', ' ').split('.')[0];
            // var temp ='2016-05-26 21:15:00';
            var dateTimeStamp = new Date(temp).valueOf();
            var minute = 1000 * 60;
            var hour = minute * 60;
            var day = hour * 24;
            var halfamonth = day * 15;
            var month = day * 30;
            var now = new Date().getTime();
            var diffValue = now - dateTimeStamp;
            if (diffValue < 0) { return; }
            var monthC = diffValue / month;
            var weekC = diffValue / (7 * day);
            var dayC = diffValue / day;
            var hourC = diffValue / hour;
            var minC = diffValue / minute;
            if (monthC >= 1) {
                return "" + parseInt(monthC) + "月以前";
            }
            else if (weekC >= 1) {
                return "" + parseInt(weekC) + "周以前";
            }
            else if (dayC >= 1) {
                return "" + parseInt(dayC) + "天以前";
            }
            else if (hourC >= 1) {
                return "" + parseInt(hourC) + "小时以前";
            }
            else if (minC >= 1) {
                return "" + parseInt(minC) + "分钟以前";
            } else
                return "刚刚";

            return '刚刚';
        };
    });
})();
//时间格式修改
(function () {
    'use strict';
 
appModule.filter("rmb", ['$filter', function ($filter) {
    return function (input) {
        if (input >0) {
            return $filter('currency')(input*100 / 100, '￥')
        } else {
            return input
        }
    }
}]);

})();



//字符串截取
(function () {
    'use strict';
    appModule.filter('cut', function () {
        return function (value, wordwise, max, tail) {
            if (!value) return '';
            max = parseInt(max, 10);
            if (!max) return value;
            if (value.length <= max) return value;

            value = value.substr(0, max);
            if (wordwise) {
                var lastspace = value.lastIndexOf(' ');
                if (lastspace != -1) {
                    value = value.substr(0, lastspace);
                }
            }

            return value + (tail || ' …');
        };
    });
})();

