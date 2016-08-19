using System.ComponentModel.DataAnnotations;

namespace LibraryAssistantApp.Models
{
    [MetadataType(typeof(RegisteredPersonMetadata))]
    public partial class Registered_Person
    {
    }

    [MetadataType(typeof(PersonLevelMetadata))]
    public partial class Person_Level
    {
    }

    [MetadataType(typeof(PersonTitleMetadata))]
    public partial class Person_Title
    {
    }

    [MetadataType(typeof(TopicMetadata))]
    public partial class Topic
    {
    }

    [MetadataType(typeof(DocumentRepositoryMetadata))]
    public partial class Document_Repository
    {
    }

    [MetadataType(typeof(DocumentCategoryMetadate))]
    public partial class Document_Category
    {
    }

    [MetadataType(typeof(DocumentExtensionMetadata))]
    public partial class Document_Extension
    {
    }

    [MetadataType(typeof(DocumentTypeMetadata))]
    public partial class Document_Type
    {
    }

    [MetadataType(typeof(CampusMetadata))]
    public partial class Campus
    {
    }
}