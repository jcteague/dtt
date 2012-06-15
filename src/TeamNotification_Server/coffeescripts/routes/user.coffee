entity_factory = require('support').core.entity_factory
methods = {}

methods.get_user = (req, res) ->
    r =
        links: [
            {"rel":"self", "href":"/user"},
            {"rel":"rooms", "href": "/user/rooms"}
        ]
    res.json(r)

methods.get_user_rooms = (req, res) ->
    entity_factory.get('ChatRoom').find '', 'id asc', (entities) ->
        rooms = (rel: room.name, href: "/rooms/#{room.id}" for room in entities)
        links = [{"rel":"self", "href": "/user/rooms" }].concat rooms
        r =
            links: links
        res.json(r)

module.exports =
    methods: methods
    build_routes: (app) ->
        app.get('/user',methods.get_user)
        app.get('/user/rooms',methods.get_user_rooms)
