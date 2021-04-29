import React, { Component } from 'react';
import { Button, Container, Heading, Form as form } from 'react-bulma-components';
import { createRecipe, uploadFiles } from '../models/recipes';
import { Form as FinalForm, Field as FinalField } from 'react-final-form';
import { InputError } from './InputError';

const { Input, Field, Control, Label, Textarea, InputFile } = form;

export class CreateRecipe extends Component {

    constructor(props) {
        super(props);
        this.state = {
            title: '',
            description: '',
            instructions: '',
            ingredients: '',
            servings: 1,
            recipeAlbumDocumentID: null
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;
        this.setState({
            [name]: value
        });
    }

    onFileChange = async event => {
        const recipeAlbumDocumentID = await uploadFiles(event.target.files);
        this.setState({
            ...this.state,
            recipeAlbumDocumentID: recipeAlbumDocumentID
        });
    }

    async handleSubmit() {
        try {
            await createRecipe(this.state);
            this.props.history.push("/");
        }
        catch (e) {
            if (e.response.status === 400)
                return e.response.data.businessErrors.reduce((acc, v) => {
                    if (!acc[v.propertyName]) acc[v.propertyName] = [v.message];
                    else acc[v.propertyName].push(v.message);
                    return acc;
                }, {});

            throw e;
        }
    }

    render() {
        return (
            <Container>
                <Heading size={3}>Create Recipe</Heading>
                <FinalForm
                    onSubmit={this.handleSubmit}
                    render={({ handleSubmit }) => (
                        <form onSubmit={handleSubmit}>
                            <FinalField name="title">
                                {({ meta }) => (
                                    <Field>
                                        <Control>
                                            <Label>Title</Label>
                                            <Input
                                                type="text"
                                                name="title"
                                                value={this.state.title}
                                                onChange={this.handleChange}
                                                className={meta.submitError ? "is-danger" : ""} />
                                        </Control>
                                        { meta.submitError && <InputError errors={meta.submitError} />}
                                    </Field>
                                )}
                            </FinalField>
                            <Field>
                                <Control>
                                    <Label>Number of servings</Label>
                                    <Input
                                        type="number"
                                        min="1"
                                        name="servings"
                                        value={this.state.servings}
                                        onChange={this.handleChange} />
                                </Control>
                            </Field>
                            <FinalField name="recipeAlbumDocumentID">
                                {({ meta }) => (
                                    <Field>
                                        <Label>Pictures</Label>
                                        <InputFile name="pictures" boxed onChange={this.onFileChange} />
                                        { meta.submitError && <InputError errors={meta.submitError} />}
                                    </Field>
                                )}
                            </FinalField>
                            <Field>
                                <Label>Description</Label>
                                <Control>
                                    <Textarea
                                        name="description"
                                        value={this.state.description}
                                        onChange={this.handleChange} />
                                </Control>
                            </Field>
                            <Field>
                                <Label>Instructions</Label>
                                <Control>
                                    <Textarea
                                        name="instructions"
                                        value={this.state.instructions}
                                        onChange={this.handleChange} />
                                </Control>
                            </Field>
                            <Field>
                                <Label>Ingredients</Label>
                                <Control>
                                    <Textarea
                                        name="ingredients"
                                        value={this.state.ingredients}
                                        onChange={this.handleChange} />
                                </Control>
                            </Field>
                            <Button className="is-success" type="submit">Create</Button>
                        </form>
                    )} />
            </Container>
        );
    }
}