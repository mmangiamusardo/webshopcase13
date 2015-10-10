angular.module("wscApp")
    .service('srvShop', function ($http, $q, $timeout) {

    var sdo = {

        getArticles: function (pageIndex, pageSize) {
            // remove this ASAP
            var paging = "0/0";
            if (pageIndex != undefined && pageSize != undefined) {
                paging = pageIndex + '/' + pageSize;
            }

            var promise = $http({
                method: 'GET',
                url: 'http://localhost:50040/api/products/page/' + paging
            }).success(function (data, status, headers, config) {
                return data;
            });

            return promise;
        },

        getArticle: function (id) {
            var promise = $http({
                method: 'GET',
                url: 'http://localhost:50040/api/products/' + id
            }).success(function (data, status, headers, config) {
                return data;
            });

            return promise;
        },

        postOrder: function (order) {

            var deferred = $q.defer();
            $http.post('http://localhost:50040/api/orders', order)
               .then(function (response) {
                   // promise is fulfilled
                   //deferred.resolve(response.data);

                   $timeout(function () {
                       deferred.resolve(response.data);
                   }, 2000);

                   // promise is returned
                   return deferred.promise;
               }, function (response) {
                   // the following line rejects the promise
                   deferred.reject(response);

                   //$timeout(function () {
                   //    deferred.reject(response);
                   //}, 2000);

                   // promise is returned
                   return deferred.promise;
               });
        },

        postOrder2: function (order) {
            var promise = $http({
                method: 'POST',
                url: 'http://localhost:50040/api/orders',
                data: order
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (response, status) {
                return response;
            });
            return promise;
        },

        getOrder: function (id) {
            var promise = $http({ method: 'GET', url: 'http://localhost:50040/api/order/' + id })
                            .success(function (data, status, headers, config) {
                                return data;
                            })
                            .error(function (response, status) {
                                console.log("The request failed with response " + response + " and status code " + status);
                            });
            return promise;
        }
    };
    return sdo;

});