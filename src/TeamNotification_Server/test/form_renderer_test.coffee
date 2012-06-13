#mocha = require("mocha")
$ = require("jquery")
should = require("should")
sinon = require("sinon")

FormTemplateRenderer = (collection) ->
    textFieldBuilder = (template) ->
        return [$('<label>', {"for":template.name}), $('<input>',{"type":"text","name":template.name})]
    templateFieldBuilder = (template) ->
        return
    generatorSelector = (fieldType) ->
        return textFieldBuilder if fieldType is 'string'
    form = $('<form>', {action:collection.uri})
    form_templates = collection.template.data
    #console.log( form_templates)
    form_templates.forEach (template) ->
        fieldGenerator = generatorSelector(template.type)
        fieldElements = fieldGenerator(template)
        #console.log(fieldElements)
        fieldElements.forEach((f) -> form.append(f))
    return form

describe 'FormTemplateRenderer', ->

    collection = null
    form = null
    field_name = null

    beforeEach (done) ->
        field_name = 'fieldA'
        collection = 
            uri: 'collectionuri'
            data: []
            template:
                data: [{name:field_name, type:'string',label:'label'}]

        form = FormTemplateRenderer(collection)
        done()

    it 'should return a form html tag', (done) ->
        form.attr('action').should.equal(collection.uri)
        done()

    it 'should render a label with the label field', (done) ->
        label = form.find('label')
        label.should.not.be.empty
        label.attr('for').should.equal field_name
        done()

    it 'should render an input with the name of the data name', (done) ->
        form.find('input').attr('name').should.equal field_name
        done()

    it 'should render an input with the type of text', (done) ->
        form.find('input').attr('type').should.equal 'text'
        done()

