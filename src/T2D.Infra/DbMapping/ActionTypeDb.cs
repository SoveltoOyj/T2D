using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using T2D.Entities;

namespace T2D.Infra
{
	public class ActionTypeDb
	{
		public static void SetDbMapping(ModelBuilder modelBuilder)
		{
			var tbl = modelBuilder.Entity<ActionTypeBase>();

			int value = 1;
			tbl
				.HasDiscriminator<int>("Discriminator")
				.HasValue<GenericAction>(value++)
				.HasValue<PaymentRequestAction>(value++)
				.HasValue<ReceiptRequestAction>(value++)
				.HasValue<IoTBotRequestAction>(value++)
				.HasValue<ServiceRequestAction>(value++)
				;
		}

	}
}
