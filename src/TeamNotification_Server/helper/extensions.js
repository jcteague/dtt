module.exports = {
    methods: {},
    build_routes: function(app){}
};

/*var http = require('http');
var response = http.ServerResponse.prototype;

module.exports = function(app){
    response.send_expanded = function(req,data){
        var get_expansion_url = function(data_to_expand, action_type ){
            var array = data_to_expand.get;
            if(!array) return;

            var length = array.length;
            var obj;
            while( length-- ){
                obj = array[ length ];
                if( obj.type == action_type ) return obj.url;
            }
        }

        var get_expansion_callback = function(url){
            var matchers = app.match(url);
            if(!matchers || matchers.length == 0) return;

            var callbacks = matchers[0].callbacks;
            if(!callbacks || callbacks.length == 0) return;
            var callback = callbacks[0];
            return callback;
        };

        var expand = function(expand,data_to_expand){
            var expansion_url = get_expansion_url(data_to_expand,expand);
            if(!expansion_url) return data_to_expand;

            var expansion_callback = get_expansion_callback(expansion_url);
            if(!expansion_callback) return data_to_expand;

            var expansion;
            var expansion_setter = function(x){expansion=x};
            expansion_callback(req,{ send: expansion_setter, send_expanded: expansion_setter});

            if(!expansion) return data_to_expand;

            data_to_expand[expand] = expansion;
            return data_to_expand;
        }

        var expand_param = req.param("expand");

        if(expand_param) data = expand(expand_param,data);
        this.send.call(this,data);
    };
}
*/
