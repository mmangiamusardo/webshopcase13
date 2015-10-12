angular.module("wscApp")
    .service('srvArticle', function ($http, articleSrvUrl) {

        var srv = {

            getArticles: function (pageIndex, pageSize) {
                // remove this ASAP
                var paging = "0/0";
                if (pageIndex != undefined && pageSize != undefined) {
                    paging = pageIndex + '/' + pageSize;
                }

                var promise = $http({
                    method: 'GET',
                    url: articleSrvUrl + 'page/' + paging
                })
                .success(function (data, status, headers, config) {
                    return data;
                })
                .error(function (response, status) {
                    console.log("The request failed with response " + response + " and status code " + status);
                });

                return promise;
            },

            getArticle: function (id) {
                var promise = $http({
                    method: 'GET',
                    url: articleSrvUrl + id
                })
                .success(function (data, status, headers, config) {
                    return data;
                })
                .error(function (response, status) {
                    console.log("The request failed with response " + response + " and status code " + status);
                });

                return promise;
            }
        }

        return srv;
    });