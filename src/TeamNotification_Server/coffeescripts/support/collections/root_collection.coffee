class RootCollection

    constructor: () ->

    to_json: ->
        return {
            links: [
                {"name": "self", "rel": "self", "href": "/" },
                {"name": "Login", "rel": "login", "href": "/user/login" },
                {"name": "Registration", "rel": "registration", "href": "/registration" }
            ]
        }

module.exports = RootCollection
