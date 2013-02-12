define 'repository_selection_view', ['general_view','form_view','breadcrumb_view'], (GeneralView, FormView, BreadcrumbView) ->

    class RepositorySelectionView extends GeneralView
    
        id: 'repository-selection-container'
        
        initialize: ->
            breadcrumb_links = [
                {name:'Home', href:'/user'}
                {name:'Associate Repositories', href:'', rel:'active'}
            ]
            @model.set('breadcrumb', breadcrumb_links)
            @breadcrumb = new BreadcrumbView(model:@model)
            @form_view = new FormView(model:@model)
            @form_view.on 'messages:display', (messages) =>
                @trigger 'messages:display', messages

        render: ->
            @$el.empty()
            @breadcrumb.render().append_to(@$el)
            @form_view.render().append_to @$el
            @delegateEvents(@events)
            @

        append_to: (parent) ->
            @$el.appendTo parent
