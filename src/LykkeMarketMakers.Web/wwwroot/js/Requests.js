/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/bootstrap/bootstrap.d.ts" />
/// <reference path="typings/lib.d.ts" />
/// <reference path="ui.ts" />
var Requests = (function () {
    function Requests() {
    }
    Requests.doRequest = function (o) {
        var _this = this;
        if (!o.url) {
            throw 'url is empty';
        }
        o.params = o.formId ? $(o.formId).serialize() : o.params;
        if (o.putToLast) {
            this.setRefreshUrl(o);
        }
        if (o.partialUpdate) {
            this.setUpdateInfo(o);
        }
        if (o.onStart) {
            o.onStart();
        }
        this.blockOnRequest();
        if (o.divResult) {
            $(o.divResult).fadeOut(200, function () { return _this.peformRequest(o); });
        }
        else {
            this.peformRequest(o);
        }
    };
    Requests.setRefreshAsReloadPage = function () {
        this.reloadAction.reloadPage = true;
    };
    Requests.setRefreshUrl = function (r) {
        this.reloadAction.reloadPage = false;
        this.reloadAction.div = r.divResult;
        this.reloadAction.url = r.url;
        this.reloadAction.data = r.params;
        this.reloadAction.showLoading = r.showLoading;
    };
    Requests.setUpdateInfo = function (r) {
        this.reloadAction.reloadPage = false;
        this.reloadAction.div = r.divResult;
        this.reloadAction.url = r.requestUrl;
        this.reloadAction.data = r.requestParams;
        this.reloadAction.showLoading = r.showLoading;
    };
    Requests.peformRequest = function (o) {
        var _this = this;
        if (o.onShowLoading) {
            o.onShowLoading();
        }
        else {
            if (o.showLoading !== false && o.divResult) {
                $(o.divResult).html('<div style="text-align:center; margin-top:20px;"><img src="/Images/Loading-pa.gif"/></div>');
                $(o.divResult).show();
            }
        }
        var options = { url: o.url, type: 'POST', data: o.params };
        $.ajax(options)
            .then(function (result) {
            if (o.onHideLoading)
                o.onHideLoading();
            if (o.onFinish)
                o.onFinish();
            if (o.divResult)
                $(o.divResult).hide();
            _this.unBlockOnRequest();
            _this.requestOkHandler(o, result);
        })
            .fail(function (jqXhr) {
            if (o.onHideLoading)
                o.onHideLoading();
            if (o.onFinish)
                o.onFinish();
            _this.unBlockOnRequest();
            _this.requestFailHandle(o, jqXhr.responseText, jqXhr);
        });
    };
    Requests.requestOkHandler = function (o, result) {
        var _this = this;
        if (result.status === 'Fail') {
            if (result.divError) {
                o.divError = result.divError;
            }
            o.placement = result.placement;
            this.requestFailHandle(o, result.msg);
            return;
        }
        if (result.status === 'Reload') {
            this.ui.hideDialog();
            if (this.reloadAction.reloadPage)
                location.reload();
            else
                this.doRequest({ url: this.reloadAction.url, params: this.reloadAction.data, divResult: this.reloadAction.div, showLoading: this.reloadAction.showLoading });
            return;
        }
        if (result.refreshUrl) {
            this.ui.hideDialog();
            this.doRequest({ url: result.refreshUrl, params: result.prms, divResult: result.div, putToHistory: result.putToHistory });
            return;
        }
        if (result.inputId) {
            $(result.inputId).val(result.text);
            return;
        }
        if (result.status === 'Request') {
            this.doRequest({ url: result.url, params: result.prms, putToHistory: result.putToHistory });
            return;
        }
        if (result.status === 'refreshLast') {
            this.last();
            return;
        }
        if (result.status === 'OkAndNothing') {
            if (o.callBack)
                o.callBack(result);
            return;
        }
        if (result.status === 'ShowDialog') {
            this.ui.hideDialog(function () {
                _this.ui.showDialog(result.url, result.prms);
            });
        }
        if (result.status === 'Redirect') {
            if (result.Params) {
                window.location = (result.Url + '?' + result.Params);
            }
            else
                window.location = result.Url;
            return;
        }
        if (result.status === 'CloseDialog') {
            this.ui.hideDialog();
            return;
        }
        if (o.divResult) {
            if (o.replaceDiv) {
                $(o.divResult).replaceWith(result);
            }
            else {
                $(o.divResult).html(result);
                $(o.divResult).fadeIn(200, function () {
                    _this.pageManager.ressize();
                });
            }
            this.pageManager.ressize();
            this.ui.initFocus();
        }
    };
    Requests.handleErrorReq = function (o, result) {
        if (result.status === 'Fail') {
            this.ui.showError(result.divError, result.Result, undefined, result.Placement);
        }
        else if (result.responseText && o.divError) {
            this.ui.showError(o.divError, result.responseText);
        }
    };
    Requests.requestFailHandle = function (o, text, jqXhr) {
        if (jqXhr) {
            if (jqXhr.status === 403) {
                window.location.reload();
                return;
            }
            if (jqXhr.status === 401) {
                this.doRequest({ url: '/Errors/AccessDenied', divResult: '#pamain', showLoading: false });
                return;
            }
        }
        else {
            this.ui.showError(o.divError, text, undefined, o.placement);
        }
    };
    Requests.blockOnRequest = function () {
        $('.disableOnRequest').each(function () {
            $(this).attr('disabled', 'true');
        });
        $('.hideOnRequest').each(function () {
            $(this).css('display', 'none');
        });
        $('.showOnRequest').each(function () {
            $(this).css('display', 'inline');
        });
    };
    Requests.unBlockOnRequest = function () {
        $('.disableOnRequest').each(function () {
            $(this).removeAttr('disabled');
        });
        $('.hideOnRequest').each(function () {
            $(this).css('display', 'inline');
        });
        $('.showOnRequest').each(function () {
            $(this).css('display', 'none');
        });
    };
    Requests.reloadAction = {
        reloadPage: true,
        url: '',
        data: undefined,
        div: '',
        showLoading: true
    };
    return Requests;
}());
var requests = Requests;
