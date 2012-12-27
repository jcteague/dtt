class LoginFormCollection

    constructor: ->

    to_json: ->
        return {
            'href': '/user/login'
            'links' : [
              {"name": "self", "rel": "login", 'href':'/user/login'}
              {"name": "forgot password", "rel": "forgot_password", 'href':'/forgot_password'}
            ]
            'template':
                'data':[
                    {'name':'username', 'label':'Email', 'type':'string'}
                    {'name':'login_password', 'label':'Password', 'type':'password'}
                ]
        }

module.exports = LoginFormCollection
