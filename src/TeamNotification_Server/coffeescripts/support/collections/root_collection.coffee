RegistrationCollection = require('./registration_collection')
LoginFormCollection = require('./login_form_collection')

class RootCollection

    constructor: () ->

    to_json: ->
        obj1 = {
            root:
                links: [
                    {"name": "self", "rel": "self", "href": "/" },
                    {"name": "Login", "rel": "login", "href": "/user/login" },
                    {"name": "Registration", "rel": "registration", "href": "/registration" }
                ]
            registration: new RegistrationCollection().to_json()
            login: new LoginFormCollection().to_json()
        }
        obj1
    
module.exports = RootCollection
