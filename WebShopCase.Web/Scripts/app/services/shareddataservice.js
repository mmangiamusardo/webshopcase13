angular.module("wscApp")
    .service('srvSharedData', function () {

        var data = {
                Order: {},
                Customer: {}
        };

        return {
            getOrder: function () {
                return data.Order;
            },
            setOrder: function (inOrder) {
                data.Order = inOrder;
            },

            getCustomer: function () {
                return data.Customer;
            },
            setCustomer: function (customer) {
                data.Customer = customer;
            }
        };
});