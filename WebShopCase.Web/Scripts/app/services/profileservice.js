angular.module("wscApp")
    .service('srvProfile', function ($http, tokenKey) {
    var srv = {
        post: function (profile) {

            var promise = $http({
                method: 'POST',
                url: 'http://localhost:50040/api/customers',
                data: profile,
                headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem(tokenKey) }
            });
            return promise;
        },

        put: function (profile) {

            var promise = $http({
                method: 'PUT',
                url: 'http://localhost:50040/api/customers',
                data: JSON.stringify(profile),
                headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem(tokenKey) }
            });
            return promise;
        },

        get: function (username)
        {
            var promise = $http({
                method: 'GET',
                url: 'http://localhost:50040/api/customers/' + username,
                headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem(tokenKey) }
            });
            return promise;
        }

    };
    return srv;
});