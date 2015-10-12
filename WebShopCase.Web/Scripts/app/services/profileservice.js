angular.module("wscApp")
    .service('srvProfile', function ($http, tokenKey, profileSrvUrl) {
    var srv = {
        post: function (profile) {

            var promise = $http({
                method: 'POST',
                url: profileSrvUrl,
                data: profile,
                headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem(tokenKey) }
            });
            return promise;
        },

        put: function (profile) {

            var promise = $http({
                method: 'PUT',
                url: profileSrvUrl,
                data: JSON.stringify(profile),
                headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem(tokenKey) }
            });
            return promise;
        },

        get: function (username)
        {
            var promise = $http({
                method: 'GET',
                url: profileSrvUrl + username,
                headers: { 'Authorization': 'Bearer ' + sessionStorage.getItem(tokenKey) }
            });
            return promise;
        }

    };
    return srv;
});