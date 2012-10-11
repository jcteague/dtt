# ENTRY:
#   template: {
#       type: "type"
#       data: [
#           
#       ]
#           id: {
#               type: "type"
#               value: "value"
#               rules: [
#                   
#               ]
#           },
#           name: {
#               type: "type"
#               value: "value"
#               rules: [
#                   
#               ]
#           }
#       }
#   }    
#
# OUTPUT:
#
    #template:
        #type: 'user_edit'
        #data: [
            #{name: 'id', value: @data.id, type: 'hidden', rules: {required: true}}
            #{name: 'first_name', label: 'First Name', value: @data.first_name, type: 'string', rules: {required: true}}
            #{name: 'last_name', label: 'Last Name', value: @data.last_name, type: 'string', rules: {required: true}}
            #{name: 'email', label: 'Email', value: @data.email, type: 'string', rules: {required: true, email: true}}
            #{name: 'password', label: 'Password', type: 'password', rules: {required: false, minlength: 6}}
            #{name: 'confirm_password', label: 'Confirm Password', type: 'password', rules: {required: false, minlength: 6, equalTo: 'password'}}
        #]



#key_value_mapper = ()

map = (template) ->
    template_data = []
    for key, value of template.data
        template_data.push {
            name: key
            label: value.label
            value: value.value
            type: value.type
            rules: value.rules
        }

    return {
        type: template.type
        data: template_data
    }

module.exports =
    map: map

