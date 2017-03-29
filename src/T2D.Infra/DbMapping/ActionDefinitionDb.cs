using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using T2D.Entities;

namespace T2D.Infra
{
	public class ActionDefinitionDb
	{
		public static void SetDbMapping(ModelBuilder modelBuilder)
		{
			var tbl = modelBuilder.Entity<ActionDefinition>();
			
			int value = 1;
			tbl
				.HasDiscriminator<int>("Discriminator")
				.HasValue<GenericAction>(value++)
				.HasValue<PaymentRequestAction>(value++)
				.HasValue<ReceiptRequestAction>(value++)
				.HasValue<IoTBotRequestAction>(value++)
				.HasValue<ServiceRequestAction>(value++)
				;

			tbl.HasOne(e => e.Object_Thing)
				.WithMany()
				.OnDelete(DeleteBehavior.Restrict)
				;
			tbl.HasOne(e => e.Alarm_Thing)
				.WithMany()
				.OnDelete(DeleteBehavior.Restrict)
				;
			tbl.HasOne(e => e.Operator_Thing)
				.WithMany()
				.OnDelete(DeleteBehavior.Restrict)
				;

			tbl.HasOne(e => e.ServiceDefinition)
							.WithMany(sd=>sd.Actions )
							.OnDelete(DeleteBehavior.Restrict)
							;

		}

	}
}
