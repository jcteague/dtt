define 'repository_selection_view', ['general_view','form_template_renderer'], (GeneralView, FormTemplateRenderer) ->

    class RepositorySelectionView extends GeneralView
    
        id: 'repository-selection-container'
        
        initialize: ->
            @form_template_renderer = new FormTemplateRenderer()

        render: ->
            @$el.empty()
            if @model.has('template')
                @$el.append(@form_template_renderer.render(@model.attributes))
            @delegateEvents(@events)
            @

        append_to: (parent) ->
            @$el.appendTo parent
