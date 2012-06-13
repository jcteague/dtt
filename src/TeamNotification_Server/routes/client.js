var methods = {};
methods.get_client = function(req, res) {};

module.exports = {
    methods: methods,
    build_routes: function(app) {
        app.get('/client', methods.get_client);

    }
};
