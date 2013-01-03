class UserEditCollection

    constructor: (@data) ->


    to_json: ->
        self = "/user/#{@data.id}/edit"
        return {
            href: self
            links: [
                {rel: 'UserEdit', name: 'self', href: self}
            ]
            template:
                href: self
                type: 'user_edit'
                data: [
                    {name: 'id', value: @data.id, type: 'hidden', rules: {required: true}}
                    {name: 'first_name', label: 'First Name', value: @data.first_name, type: 'string', rules: {required: true}}
                    {name: 'last_name', label: 'Last Name', value: @data.last_name, type: 'string', rules: {required: true}}
                    {name: 'email', label: 'Email', value: @data.email, type: 'string', rules: {required: true, email: true}}
                    {name: 'password', label: 'Password', type: 'password', rules: {required: false, minlength: 6}}
                    {name: 'confirm_password', label: 'Confirm Password', type: 'password', rules: {required: false, minlength: 6, equalTo: 'password'}}
                ]
        }

module.exports = UserEditCollection
