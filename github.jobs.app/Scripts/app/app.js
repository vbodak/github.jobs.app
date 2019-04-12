var AppViewModel = function (baseSearchUrl) {
    var self = this;
    var pageSize = 50;
    self.BaseSearchUrl = baseSearchUrl;
    self.Model = ko.observable();
    self.Items = ko.observableArray([]);
    self.description = ko.observable('');
    self.location = ko.observable('');
    self.fullTime = ko.observable(false);
    self.totalCount = ko.observable(0);
    self.currentPage = ko.observable(1);
    self.showLoadMore = ko.observable(false);
    self.paging = ko.observable('');
    self.showPaging = ko.observable(false);

    var formatPaging = function () {
        var items = self.Items();
        self.paging("Showing " + ((pageSize * (self.currentPage() - 1)) + 1) + " - " + (pageSize * (self.currentPage() - 1) + items.length) + " of " + self.totalCount() + " jobs");
        self.showPaging(true);
        self.showLoadMore((self.currentPage()) * pageSize < self.totalCount());
    }

    var calculateTotal = function () {
        $.ajax({
            url: self.BaseSearchUrl + '?description=' + self.description() + '&location=' + self.location() + '&fullTime=' + self.fullTime() + "&page=1&calculateTotal=true",
            type: 'GET',
            dataType: 'json',
            processData: false,
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                self.totalCount(result.Total);
                formatPaging();
            },
            failure: function (error) {
                console.log(error);
            }
        });
    }

    var search = function() {
        $.ajax({
            url: self.BaseSearchUrl + '?description=' + self.description() + '&location=' + self.location() + '&fullTime=' + self.fullTime() + "&page=1",
            type: 'GET',
            dataType: 'json',
            processData: false,
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                self.Items(result.Items);
                calculateTotal();
            },
            failure: function (error) {
                console.log(error);
            }
        });
    }

    var loadMore = function () {
        self.currentPage(self.currentPage() + 1);

        $.ajax({
            url: self.BaseSearchUrl + '?description=' + self.description() + '&location=' + self.location() + '&fullTime=' + self.fullTime() + "&page=" + self.currentPage(),
            type: 'GET',
            dataType: 'json',
            processData: false,
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                self.Items(result.Items);
                formatPaging();
            },
            failure: function (error) {
                console.log(error);
            }
        });
    }

    self.search = search;
    self.loadMore = loadMore;
};