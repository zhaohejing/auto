var app = app || {};
(function () {

    var appLocalizationSource = abp.localization.getSource('Zhj');
    app.localize = function () {
        return appLocalizationSource.apply(this, arguments);
    };

    app.downloadTempFile = function (file) {
        location.href = abp.appPath + 'File/DownloadTempFile?fileType=' +
            file.fileType + '&fileToken='
            + file.fileToken + '&fileName=' + file.fileName;
    };


    app.downloadFile = function (file) {
        location.href = abp.appPath +
            'File/DownloadFile?file=' + file;
    };


    app.createDateRangePickerOptions = function () {
        var options = {
            locale: {
                format: 'YYYY-MM-DDT00:00:00',
                applyLabel: '确定',
                cancelLabel: '取消',
                customRangeLabel: '时间范围',
                daysOfWeek: ['日', '一', '二', '三', '四', '五', '六'],
                monthNames: ['一月', '二月', '三月', '四月', '五月', '六月',
                        '七月', '八月', '九月', '十月', '十一月', '十二月'],
                firstDay: 1
            },
            min: '2000-01-01',
            minDate: '2000-01-01',
            //max: todayAsString,
            //maxDate: todayAsString,
            ranges: {}, singleDatePicker:true
        };

        return options;
    };

    app.getUserProfilePicturePath = function (profilePictureId) {
        return profilePictureId ?
                            (abp.appPath + 'Profile/GetProfilePictureById?id=' + profilePictureId) :
                            (abp.appPath + 'Common/Images/default-profile-picture.png');
    }

    app.getUserProfilePicturePath = function () {
        return abp.appPath + 'Profile/GetProfilePicture?v=' + new Date().valueOf();
    }

    app.getShownLinkedUserName = function (linkedUser) {
        if (!abp.multiTenancy.isEnabled) {
            return linkedUser.userName;
        } else {
            if (linkedUser.tenancyName) {
                return linkedUser.tenancyName + '\\' + linkedUser.username;
            } else {
                return '.\\' + linkedUser.username;
            }
        }
    }

    app.notification = app.notification || {};

    app.notification.getUiIconBySeverity = function (severity) {
        switch (severity) {
            case abp.notifications.severity.SUCCESS:
                return 'fa fa-check';
            case abp.notifications.severity.WARN:
                return 'fa fa-warning';
            case abp.notifications.severity.ERROR:
                return 'fa fa-bolt';
            case abp.notifications.severity.FATAL:
                return 'fa fa-bomb';
            case abp.notifications.severity.INFO:
            default:
                return 'fa fa-info';
        }
    };

})();