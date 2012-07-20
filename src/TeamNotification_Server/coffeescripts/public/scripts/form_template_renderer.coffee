define 'form_template_renderer', ['jquery', 'jquery.validate'], ($, jquery_validate) ->

    class FormTemplateRenderer

        render: (collection) ->
            form_class = 'well'
            if typeof collection.form_class != 'undefined'
                form_class = collection.form_class
            form = $('<form>', {action:collection.href})
            form_templates = collection.template.data
            form_templates.forEach (template) =>
                fieldGenerator = @get_builder_for(template.type)
                fieldElements = fieldGenerator(template)
                fieldElements.forEach((f) -> form.append(f))#$('<p/>').append(f)))

            @set_up_validation(form, collection.template)
            form.attr('class', form_class)
            form.append($('<input>', {"type":"submit","class":"btn btn-primary"}))#$('<p/>').append($('<input>', {"type":"submit","class":"btn btn-primary"})))
            form

        get_builder_for: (field_type) ->
            return @textAreaBuilder if field_type is 'string-big'
            return @textFieldBuilder if field_type is 'string'
            return @hiddenFieldBuilder if field_type is 'hidden'
            return @passwordFieldBuilder if field_type is 'password'
            return @passwordWithConfirmFieldBuilder if field_type is 'password-confirmed'
        
        dropDownListBuilder:(template) ->
            dropDownList = $('<select>',{"name":template.name, "class":"input-xlarge"})
            #for (props in data) 
           #     dropDownList.append("<option value='#{props.value}'>#{props.text}</option>")
            return [$('<label>', {"for":template.name,"class":"control-label"}).text(template.label), dropDownList]

        dropDownListOptionBuilder: (props) ->
            opt = $("<option>",{value:props.value})
            opt.append(props.text)
            return opt 
            
        textFieldBuilder: (template) ->
            return [$('<label>', {"for":template.name,"class":"control-label"}).text(template.label), $('<input>',{"type":"text","name":template.name, "class":"input-xlarge"})]

        textAreaBuilder: (template) ->
            return [$('<label>', {"for":template.name, "class":"control-label"}).text(template.label), $('<textarea>',{"name":template.name,"rows":2,maxlength:template.maxlength,"class":"input-xlarge"})]

        hiddenFieldBuilder: (template) ->
            return [$('<input>',{"type":"hidden","name":template.name})]

        passwordFieldBuilder: (template) ->
            return [$('<label>', {"for":template.name}).text(template.label), $('<input>',{"id": template.name, "type":"password","name":template.name, "class":"input-xlarge"})]

        passwordWithConfirmFieldBuilder: (template) ->
            return [$('<label>', {"for":template.name}).text(template.label), $('<input>',{"type":"password","name":template.name,"class":"input-xlarge"}), $('<label>', {"for":"#{template.name}_confirm"}).text("Confirm #{template.label}"), $('<input>',{"type":"password","name":"#{template.name}_confirm","class":"input-xlarge"})]

        set_up_validation: (form, template) ->
            return unless _.find(template.data, (element) ->
                element.rules?
            )?
            rules = {}
            for field in template.data
                rules[field.name] = @get_rules(field.rules)

            messages = @get_messages_for(template.type)
            form.validate(rules: rules, messages: messages)

        get_rules: (rules) ->
            r = _.clone rules
            if r.equalTo?
                r.equalTo = "#" + rules.equalTo
            r

        get_messages_for: (template_type) ->
            if template_type is 'registration'
                return {
                    first_name:
                        required: 'First name is invalid'
                    last_name:
                        required: 'Last name is invalid'
                    email: 'Email is invalid'
                    password:
                        required: 'Password must contain at least 6 characters'
                        minlength: 'Password must contain at least 6 characters'
                    confirm_password:
                        required: 'Password must contain at least 6 characters'
                        minlength: 'Password must contain at least 6 characters'
                        equalTo: 'Password and confirmation must match'
                }
