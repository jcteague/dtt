define 'form_template_renderer', ['jquery'], ($) ->

    class FormTemplateRenderer

        render: (collection) ->
            textFieldBuilder = (template) ->
                return [$('<label>', {"for":template.name}), $('<input>',{"type":"text","name":template.name})]
            templateFieldBuilder = (template) ->
                return
            generatorSelector = (fieldType) ->
                return textFieldBuilder if fieldType is 'string'
            form = $('<form>', {action:collection.uri})
            form_templates = collection.template.data
            form_templates.forEach (template) ->
                fieldGenerator = generatorSelector(template.type)
                fieldElements = fieldGenerator(template)
                fieldElements.forEach((f) -> form.append(f))
            
            form

