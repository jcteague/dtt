class RegistrationCollection

    to_json: ->
        self = '/registration'
        return {
            href: self
            links: [
                {rel: 'Registration', name: 'self', href: self}
            ]
            template:
                type: 'registration'
                data: [
                    {name: 'first_name', label: 'First Name', type: 'string', rules: {required: true}}
                    {name: 'last_name', label: 'Last Name', type: 'string', rules: {required: true}}
                    {name: 'email', label: 'Email', type: 'string', rules: {required: true, email: true}}
                    {name: 'password', label: 'Password', type: 'password', rules: {required: true, minlength: 6}}
                    {name: 'confirm_password', label: 'Confirm Password', type: 'password', rules: {required: true, minlength: 6, equalTo: 'password'}}
                ]
        }

module.exports = RegistrationCollection
