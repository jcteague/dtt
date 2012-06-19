methods = {}
methods.get_root = (req, res) ->
    r =
        links: [
            {"name": "self", "rel": "self", "href": "/" },
            {"name": "user", "rel": "User", "href": "/user" }
        ]
    res.json(r)

module.exports =
    methods: methods,
    build_routes: (app) ->
        app.get('/',methods.get_root)
