methods = {}
methods.get_root = (req, res) ->
    r =
        links: [
            {"rel": "self", "href": "/" },
            {"rel": "user", "href": "/user" }
        ]
    res.json(r)

module.exports =
    methods: methods,
    build_routes: (app) ->
        app.get('/',methods.get_root)
