using System;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;

namespace ArcadeBot.TypeReaders
{
    //internal class UriReader : TypeReader
    //{
    //    public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
    //    {

    //        if (Uri.TryCreate(input, UriKind.RelativeOrAbsolute, out Uri? result))
    //            return Task.FromResult(TypeReaderResult.FromSuccess(result));

    //        return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Input could not be parsed as a URI."));
    //    }
    //}
    internal class UriConverter : TypeConverter
    {
        //public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, string input, IServiceProvider services)
        //{

        //    if (Uri.TryCreate(input, UriKind.RelativeOrAbsolute, out Uri? result))
        //        return Task.FromResult(TypeConverterResult.FromSuccess(result));

        //    return Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.ParseFailed, "Input could not be parsed as a URI."));
        //}
        public override bool CanConvertTo(Type type)
        {
            return true;
        }

        public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, IApplicationCommandInteractionDataOption option, IServiceProvider services)
        {
            if (Uri.TryCreate(option.Value.ToString(), UriKind.RelativeOrAbsolute, out Uri? result))
                return Task.FromResult(TypeConverterResult.FromSuccess(result));

            return Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.ParseFailed, "Input could not be parsed as a URI."));
        }

        public override ApplicationCommandOptionType GetDiscordType()
        {
            return ApplicationCommandOptionType.String;
        }
    }
}
