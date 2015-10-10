angular.module("wscApp")
    .service('srvAccount', function ($http, registerUrl, tokenUrl, tokenKey) {
    var srv = {

        register: function (data) {
            var request = $http.post(registerUrl, data);

            return request;
        },

        generateAccessToken: function (loginData) {
            var requestToken = $http({
                method: 'POST',
                url: tokenUrl,
                data: jQuery.param(loginData),
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
                }
            });

            return requestToken;
        },

        isUserAuthenticated: function () {
            var token = sessionStorage.getItem(tokenKey);

            if (token)
                return true;
            else
                return false;
        },

        logout: function () {
            sessionStorage.removeItem(tokenKey);
        }           
    };
    return srv;
});