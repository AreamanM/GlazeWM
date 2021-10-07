﻿using System;
using GlazeWM.Domain.Containers.Commands;
using GlazeWM.Infrastructure.Bussing;

namespace GlazeWM.Domain.Containers.CommandHandlers
{
  class SwapContainersHandler : ICommandHandler<SwapContainersCommand>
  {
    private ContainerService _containerService;

    public SwapContainersHandler(ContainerService containerService)
    {
      _containerService = containerService;
    }

    public CommandResponse Handle(SwapContainersCommand command)
    {
      var firstContainer = command.FirstContainer;
      var secondContainer = command.SecondContainer;

      if (firstContainer.Parent != secondContainer.Parent)
        throw new Exception("Attempting to swap containers with different parents. This is a bug.");

      // Keep references to the original indices.
      var firstContainerIndex = firstContainer.Index;
      var secondContainerIndex = secondContainer.Index;

      // Swap the containers. Note that using the parent of the first container is arbitrary since
      // both containers have the same parent.
      var parent = firstContainer.Parent;
      parent.Children[firstContainerIndex] = secondContainer;
      parent.Children[secondContainerIndex] = firstContainer;

      _containerService.ContainersToRedraw.Add(parent);

      return CommandResponse.Ok;
    }
  }
}
