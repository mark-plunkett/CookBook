import React, { Component } from 'react';
import { Button, Container, Heading, Form } from 'react-bulma-components';
import { createRecipe, uploadFiles } from '../models/recipes';
import { Form as FinalForm, Field as FinalField } from 'react-final-form';
import { InputError } from './InputError';
import { InstructionsInput } from './RecipeForm/InstructionsInput';
import { IngredientsInput } from './RecipeForm/IngredientsInput';
import { TitleInput } from './RecipeForm/TitleInput';
import { DescriptionInput } from './RecipeForm/DescriptionInput';
import { NumberOfServingsInput } from './RecipeForm/NumberOfServingsInput';

const { Input, Field, Control, Label, Textarea, InputFile } = Form;

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
                            <TitleInput title={this.state.title} handleChange={this.handleChange} />
                            <NumberOfServingsInput servings={this.state.servings} handleChange={this.handleChange} />
                            <FinalField name="recipeAlbumDocumentID">
                                {({ meta }) => (
                                    <Field>
                                        <Label>Pictures</Label>
                                        <InputFile name="pictures" boxed onChange={this.onFileChange} />
                                        { meta.submitError && <InputError errors={meta.submitError} />}
                                    </Field>
                                )}
                            </FinalField>
                            <DescriptionInput description={this.state.description} handleChange={this.handleChange} />
                            <InstructionsInput instructions={this.state.instructions} handleChange={this.handleChange} />
                            <IngredientsInput ingredients={this.state.ingredients} handleChange={this.handleChange} />
                            <Button className="is-success" type="submit">Create</Button>
                        </form>
                    )} />
            </Container>
        );
    }
}