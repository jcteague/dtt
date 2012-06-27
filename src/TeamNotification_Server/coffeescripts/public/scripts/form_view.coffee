define 'form_view', ['backbone', 'form_template_renderer'], (Backbone, FormTemplateRenderer) ->

    class FormView extends Backbone.View

        id: 'form-container'

        events:
            'submit': 'submit_form'

        initialize: ->
            @form_template_renderer = new FormTemplateRenderer()

        render: ->
            @$el.empty()
            if @model? and @model.template
                @$el.append('<h1>Form</h1>')
                @$el.append(@form_template_renderer.render(@model))
            @

        update: (template) ->
            @model = template

        append_to: (parent) ->
            @$el.appendTo parent

        submit_form: (event) ->
            event.preventDefault()
            url = _.find(@data.links, (item) ->
                item.name is 'self'
            )
            data = {}
            $('input').not(':submit').each () ->
                $current = $(this)
                data[$current.attr('name')] = $current.val()

            $.post(url.href, data, (res) -> console.log(res))
