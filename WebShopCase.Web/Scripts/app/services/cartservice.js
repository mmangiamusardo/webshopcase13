angular.module("wscApp")
    .service('srvCart', function () {
        var subTotals = [];
        var cart = [];
        var size = 0;
        var total = 0.0;

        var sum = function(items, prop){
            return items.reduce( function(a, b){
                return a + b[prop];
            }, 0);
        };

        return {

            add: function (id, qty, price, name, pct, vat) {
                cart.push({ productId: id, cartQty: qty, unitPrice: price, productName: name, picture: pct, vat: vat });

                // to refactor
                subTotals = cart.reduce(function (c, x) {
                    if (!c[x.productId]) {
                        c[x.productId] = {
                            productName: x.productName,
                            productId: x.productId,
                            productPct: x.picture,
                            productPrice: x.unitPrice,
                            vat: x.vat,
                            totalQty: 0,
                            total: 0,
                            totalVat:0
                        };
                    }

                    c[x.productId].totalQty += Number(x.cartQty);
                    c[x.productId].total += Number(x.unitPrice);
                    c[x.productId].totalVat += Number(x.unitPrice * x.vat);
                    return c;
                }, {});
            },
            remove: function (id) {
                for (var i = cart.length - 1; i >= 0; i--) {
                    if (cart[i].productId === id) {
                        cart.splice(i, 1);
 
                        subTotals = cart.reduce(function (c, x) {
                            if (!c[x.productId]) {
                                c[x.productId] = {
                                    productName: x.productName,
                                    productId: x.productId,
                                    productPct: x.picture,
                                    productPrice: x.unitPrice,
                                    vat: x.vat,
                                    totalQty: 0,
                                    total: 0,
                                    totalVat: 0
                                };
                            }

                            c[x.productId].totalQty += Number(x.cartQty);
                            c[x.productId].total += Number(x.unitPrice);
                            c[x.productId].totalVat += Number(x.unitPrice * x.vat);
                            return c;
                        }, {});


                        return;
                    }
                }
            },
            size: function(){
                return sum(cart, 'cartQty');
            },
            total: function () {
                return sum(cart, 'unitPrice');
            },
            totalVat: function () {
                return sum(cart, 'vat');
            },
            clear: function (){
                var subTotals = [];
                cart = [];
                sizeCart = 0;
                totalCart = 0.0;
            },
            subtotals: function () {
                return subTotals;
            }
        }
    }
);
