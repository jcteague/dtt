entity_factory = require('support').core.entity_factory
Repository = require('../support/repository')
methods = {}

methods.get_user = (req, res) ->
    #new Repository('ChatRoom').find().then((values) -> console.log values)
    # build('user_collection').for(user_id).fetch_to(callback)
    r =
        links: [
            {"rel":"self", "href":"/user"},
            {"rel":"rooms", "href": "/user/rooms"}
        ]
    res.json(r)

methods.get_user_rooms = (req, res) ->
    # build('user_collection').for(user_id).to(callback)
    # fetch('ChatRoom').for(user_id).as('user_collection').to(callback)
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
