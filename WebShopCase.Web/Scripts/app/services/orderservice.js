angular.module("wscApp")
    .service('srvOrder', function ($http, $q, tokenKey, orderSrvUrl) {

    var srv = {

        postOrder: function (order) {

            var deferred = $q.defer();
                $http({
                    method: 'POST',
                    url: orderSrvUrl,
                    data: order,
                    headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem(tokenKey) }
                })
                .then(function (response) {
                   deferred.resolve({data: response.data, status: response.status});
                   return deferred.promise;
               }, function (response) {
                   // the following line rejects the promise
                   //deferred.reject(response);

                   //better resolve. Rejecting remains pending
                   deferred.resolve({ data: response.data, status: response.status });

                   return deferred.promise;
               });
            return deferred.promise;
        },

        getOrder: function (id) {
            var promise = $http(
                {
                    method: 'GET',
                    url: orderSrvUrl + id,
                    headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem(tokenKey) }
                })
                .success(function (data, status, headers, config) {
                    return data;
                })
                .error(function (response, status) {
                    console.log("The request failed with response " + response + " and status code " + status);
                });
            return promise;
        }
    };
    return srv;

});