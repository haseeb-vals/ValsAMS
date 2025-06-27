# MultiSaaS API Documentation
## API Endpoints
### Form Render API
#### Get Form Data

```
GET /api/FormRenderApi/{formDefinitionId}?recordId={recordId}
```

Returns the data needed to render a form. The `recordId` parameter is optional and should be provided when editing an existing record.

#### Validate Form Submission

```
POST /api/FormRenderApi/validate
```

Validates a form submission without saving it to the database.

#### Submit Form

```
POST /api/FormRenderApi/submit
```

Submits a form and saves the data to the database.

### Form Definition API
#### Get All Form Definitions

```
GET /api/FormDefinitionApi
```

Returns a list of all form definitions.

#### Get Form Definition

```
GET /api/FormDefinitionApi/{id}
```

Returns a specific form definition by ID.

#### Create Form Definition

```
POST /api/FormDefinitionApi
```

Creates a new form definition.

#### Update Form Definition

```
PUT /api/FormDefinitionApi/{id}
```

Updates an existing form definition.

#### Delete Form Definition

```
DELETE /api/FormDefinitionApi/{id}
```

Deletes a form definition.

### Form Field API
#### Get All Form Fields

```
GET /api/FormFieldApi/{formDefinitionId}
```

Returns a list of all fields for a specific form definition.

#### Get Form Field

```
GET /api/FormFieldApi/{id}
```

Returns a specific form field by ID.

#### Create Form Field

```
POST /api/FormFieldApi
```

Creates a new form field.

#### Update Form Field

```
PUT /api/FormFieldApi/{id}
```

Updates an existing form field.

#### Delete Form Field

```
DELETE /api/FormFieldApi/{id}
```

Deletes a form field.

### Form Record API
#### Get All Records for a Form

```
GET /api/FormRecordApi/{formDefinitionId}/records
```

Returns a list of all records for a specific form definition.

#### Get Specific Record

```
GET /api/FormRecordApi/{formDefinitionId}/records/{recordId}
```

Returns a specific record for a form definition.

#### Delete Record

```
DELETE /api/FormRecordApi/{formDefinitionId}/records/{recordId}
```

Deletes a specific record for a form definition.