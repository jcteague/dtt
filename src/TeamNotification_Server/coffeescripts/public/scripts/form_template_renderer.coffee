define 'form_template_renderer', ['jquery'], ($) ->

    class FormTemplateRenderer

        render: (collection) ->
            textFieldBuilder = (template) ->
                return [$('<label>', {"for":template.name}).text(template.label), $('<input>',{"type":"text","name":template.name})]
            textAreaBuilder = (template) ->
                return [$('<label>', {"for":template.name}).text(template.label), $('<textarea>',{"name":template.name,"rows":5,"col":34,maxlength:template.maxlength})]
            hiddenFieldBuilder = (template) ->
                return [$('<input>',{"type":"hidden","name":template.name})]
            templateFieldBuilder = (template) ->
                return

            generatorSelector = (fieldType) ->
                return textAreaBuilder if fieldType is 'string-big'
                return textFieldBuilder if fieldType is 'string'
                return hiddenFieldBuilder if fieldType is 'hidden'

            form = $('<form>', {action:collection.href})
            form_templates = collection.template.data
            form_templates.forEach (template) ->
                fieldGenerator = generatorSelector(template.type)
                fieldElements = fieldGenerator(template)
                fieldElements.forEach((f) -> form.append(f))
            form.append($('<input>', {"type":"submit"}))
            form
