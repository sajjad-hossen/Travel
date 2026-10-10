using Microsoft.EntityFrameworkCore;
using TravelBD.Application.Security;
using TravelBD.Domain.Entities;
using TravelBD.Domain.Enums;

namespace TravelBD.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(TravelDbContext context)
    {
        if (!await context.Users.AnyAsync(u => u.Role == UserRole.Admin))
        {
            context.Users.Add(new User
            {
                Name = "Admin",
                Email = "admin@travelbd.com",
                PasswordHash = PasswordHasher.Hash("Admin@12345"),
                Role = UserRole.Admin
            });
            await context.SaveChangesAsync();
        }

        if (await context.Locations.AnyAsync())
        {
            return; // Already seeded
        }

        // 1. LOCATIONS
        var dhaka = new Location
        {
            Name = "Dhaka",
            BanglaName = "ঢাকা",
            Slug = "dhaka",
            Type = LocationType.TransitHub,
            District = "Dhaka",
            Division = "Dhaka",
            Latitude = 23.8103,
            Longitude = 90.4125,
            IsMajorHub = true,
            IsTouristDestination = false,
            Description = "Capital city and the primary transit epicenter connecting every corner of Bangladesh.",
            HeroImageUrl = "https://images.unsplash.com/photo-1608958435020-e8a7109ba809?w=1200"
        };

        var chittagong = new Location
        {
            Name = "Chittagong",
            BanglaName = "চট্টগ্রাম",
            Slug = "chittagong",
            Type = LocationType.TransitHub,
            District = "Chattogram",
            Division = "Chattogram",
            Latitude = 22.3569,
            Longitude = 91.7832,
            IsMajorHub = true,
            IsTouristDestination = true,
            Description = "The commercial capital and primary gateway to the Southeastern Hill Tracts and Bay of Bengal beaches.",
            HeroImageUrl = "https://images.unsplash.com/photo-1596895111956-bf1cf0599ce5?w=1200"
        };

        var coxsBazar = new Location
        {
            Name = "Cox's Bazar",
            BanglaName = "কক্সবাজার",
            Slug = "coxs-bazar",
            Type = LocationType.TouristSpot,
            District = "Cox's Bazar",
            Division = "Chattogram",
            Latitude = 21.4272,
            Longitude = 92.0058,
            IsMajorHub = true,
            IsTouristDestination = true,
            Description = "World's longest unbroken natural sea beach stretching over 120 kilometers with breathtaking sunsets.",
            HeroImageUrl = "https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=1200"
        };

        var bandarban = new Location
        {
            Name = "Bandarban",
            BanglaName = "বান্দরবান",
            Slug = "bandarban",
            Type = LocationType.TouristSpot,
            District = "Bandarban",
            Division = "Chattogram",
            Latitude = 22.1953,
            Longitude = 92.2184,
            IsMajorHub = false,
            IsTouristDestination = true,
            Description = "The crown jewel of the Chittagong Hill Tracts featuring misty peaks, indigenous culture, and dramatic cascades.",
            HeroImageUrl = "https://images.unsplash.com/photo-1544620347-c4fd4a3d5957?w=1200"
        };

        var rangamati = new Location
        {
            Name = "Rangamati",
            BanglaName = "রাঙ্গামাটি",
            Slug = "rangamati",
            Type = LocationType.TouristSpot,
            District = "Rangamati",
            Division = "Chattogram",
            Latitude = 22.6533,
            Longitude = 92.1753,
            IsMajorHub = false,
            IsTouristDestination = true,
            Description = "Scenic lake district nestled around the emerald waters of Kaptai Lake with tribal handlooms and peaceful boat tours.",
            HeroImageUrl = "https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=1200"
        };

        var sylhet = new Location
        {
            Name = "Sylhet",
            BanglaName = "সিলেট",
            Slug = "sylhet",
            Type = LocationType.District,
            District = "Sylhet",
            Division = "Sylhet",
            Latitude = 24.8949,
            Longitude = 91.8687,
            IsMajorHub = true,
            IsTouristDestination = true,
            Description = "Northeastern haven of rolling tea estates, Ratargul swamp forest, and spiritual shrines.",
            HeroImageUrl = "https://images.unsplash.com/photo-1578632767115-351597cf2477?w=1200"
        };

        var rajshahi = new Location
        {
            Name = "Rajshahi",
            BanglaName = "রাজশাহী",
            Slug = "rajshahi",
            Type = LocationType.District,
            District = "Rajshahi",
            Division = "Rajshahi",
            Latitude = 24.3745,
            Longitude = 88.6042,
            IsMajorHub = true,
            IsTouristDestination = false,
            Description = "The Silk City on the bank of the Padma River in northwest Bangladesh.",
            HeroImageUrl = "https://images.unsplash.com/photo-1518684079-3c830dcef090?w=1200"
        };

        var khulna = new Location
        {
            Name = "Khulna",
            BanglaName = "খুলনা",
            Slug = "khulna",
            Type = LocationType.District,
            District = "Khulna",
            Division = "Khulna",
            Latitude = 22.8456,
            Longitude = 89.5403,
            IsMajorHub = true,
            IsTouristDestination = false,
            Description = "Gateway to the mangrove expanse of the Sundarbans.",
            HeroImageUrl = "https://images.unsplash.com/photo-1513836279014-a89f7a76ae86?w=1200"
        };

        var barisal = new Location
        {
            Name = "Barisal",
            BanglaName = "বরিশাল",
            Slug = "barisal",
            Type = LocationType.District,
            District = "Barishal",
            Division = "Barishal",
            Latitude = 22.7010,
            Longitude = 90.3535,
            IsMajorHub = true,
            IsTouristDestination = false,
            Description = "The Venice of Bengal famous for floating guava markets and riverine networks.",
            HeroImageUrl = "https://images.unsplash.com/photo-1473448912268-2022ce9509d8?w=1200"
        };

        var thanchi = new Location
        {
            Name = "Thanchi",
            BanglaName = "থানচি",
            Slug = "thanchi",
            Type = LocationType.TouristSpot,
            District = "Bandarban",
            Division = "Chattogram",
            Latitude = 21.7865,
            Longitude = 92.4284,
            IsMajorHub = false,
            IsTouristDestination = true,
            Description = "Gateway to deep Bandarban treks, Sangu River boat rides, Amiakhum, and Nafakhum falls.",
            HeroImageUrl = "https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?w=1200"
        };

        var sajek = new Location
        {
            Name = "Sajek Valley",
            BanglaName = "সাজেক ভ্যালি",
            Slug = "sajek-valley",
            Type = LocationType.TouristSpot,
            District = "Rangamati",
            Division = "Chattogram",
            Latitude = 23.3820,
            Longitude = 92.2938,
            IsMajorHub = false,
            IsTouristDestination = true,
            Description = "Queen of Hills above the clouds, located in Baghaichhari Upazila accessible via Dighinala army escort.",
            HeroImageUrl = "https://images.unsplash.com/photo-1486870591958-9b9d0d1dda99?w=1200"
        };

        var locations = new[] { dhaka, chittagong, coxsBazar, bandarban, rangamati, sylhet, rajshahi, khulna, barisal, thanchi, sajek };
        await context.Locations.AddRangeAsync(locations);
        await context.SaveChangesAsync();

        // 2. ROUTE SEGMENTS & TRANSPORT OPTIONS
        void AddBidirectionalLeg(Location a, Location b, double dist, int durMins, List<(TransportMode mode, RouteTier tier, string op, decimal minCost, decimal maxCost, int freq, string notes)> transports)
        {
            var segForward = new RouteSegment
            {
                OriginId = a.Id,
                DestinationId = b.Id,
                DistanceKm = dist,
                AvgDurationMinutes = durMins
            };
            var segBackward = new RouteSegment
            {
                OriginId = b.Id,
                DestinationId = a.Id,
                DistanceKm = dist,
                AvgDurationMinutes = durMins
            };

            foreach (var t in transports)
            {
                segForward.TransportOptions.Add(new TransportOption
                {
                    Mode = t.mode,
                    Tier = t.tier,
                    OperatorName = t.op,
                    MinCostBdt = t.minCost,
                    MaxCostBdt = t.maxCost,
                    FrequencyPerDay = t.freq,
                    ScheduleNotes = t.notes
                });

                segBackward.TransportOptions.Add(new TransportOption
                {
                    Mode = t.mode,
                    Tier = t.tier,
                    OperatorName = t.op,
                    MinCostBdt = t.minCost,
                    MaxCostBdt = t.maxCost,
                    FrequencyPerDay = t.freq,
                    ScheduleNotes = t.notes
                });
            }

            context.RouteSegments.AddRange(segForward, segBackward);
        }

        // Dhaka <-> Chittagong (Core Backbone)
        AddBidirectionalLeg(dhaka, chittagong, 245, 330, new()
        {
            (TransportMode.Train, RouteTier.Snigdha, "Subarna / Sonar Bangla Express", 405, 850, 4, "Intercity high-speed train via Kamalapur to Chittagong station (5.5 hrs)."),
            (TransportMode.Bus, RouteTier.AcBus, "Green Line / Hanif / Desh Travels", 1000, 1600, 24, "Day & sleeper coaches via Dhaka-Chittagong 4-lane highway."),
            (TransportMode.Bus, RouteTier.NonAcBus, "Hanif / Shyamoli Paribahan", 680, 800, 40, "Budget frequent departures from Sayedabad & Gabtoli."),
            (TransportMode.Flight, RouteTier.EconomyAir, "Biman Bangladesh / US-Bangla / Air Astra", 3500, 6500, 8, "45-minute flight from Hazrat Shahjalal (DAC) to Shah Amanat (CGP).")
        });

        // Chittagong <-> Cox's Bazar
        AddBidirectionalLeg(chittagong, coxsBazar, 150, 150, new()
        {
            (TransportMode.Train, RouteTier.Snigdha, "Cox's Bazar Express / Parjotok Express", 250, 500, 3, "Scenic new railway track over Karnaphuli and hill country (2.5 hrs)."),
            (TransportMode.Bus, RouteTier.AcBus, "S.Alam / Hanif / Saudia AC", 600, 900, 16, "Frequent AC coaches from Dampara and Cinema Palace."),
            (TransportMode.Bus, RouteTier.NonAcBus, "Purbani / Mars Paribahan", 380, 450, 30, "Fast local highway coaches.")
        });

        // Direct Dhaka <-> Cox's Bazar (Direct option)
        AddBidirectionalLeg(dhaka, coxsBazar, 395, 480, new()
        {
            (TransportMode.Train, RouteTier.Snigdha, "Cox's Bazar Express (Direct Intercity)", 750, 1725, 2, "Iconic direct overnight & day intercity train arriving at the oyster-shaped station."),
            (TransportMode.Bus, RouteTier.SleeperBus, "Saintmartin Paribahan / Green Line Sleeper", 2000, 3200, 12, "Luxury multi-axle sleeper buses departing evening/night."),
            (TransportMode.Bus, RouteTier.AcBus, "Desh Travels / Shyamoli / Ena AC", 1400, 2000, 20, "Scania & Hyundai business class."),
            (TransportMode.Flight, RouteTier.EconomyAir, "US-Bangla / Novoair / Biman", 4500, 8500, 10, "Direct 55-minute flight from Dhaka to Cox's Bazar Airport (CXB).")
        });

        // Chittagong <-> Bandarban (Primary hill gate)
        AddBidirectionalLeg(chittagong, bandarban, 75, 135, new()
        {
            (TransportMode.Bus, RouteTier.NonAcBus, "Purbani Bus / Pubali Paribahan", 180, 250, 20, "Departs from Bahaddarhat Bus Terminal every 30 minutes (2 - 2.5 hrs)."),
            (TransportMode.ChanderGari, RouteTier.ReservedVehicle, "Private Mahendra / Chander Gari / HiAce", 3500, 5000, 5, "Door to door rental ideal for groups travelling with heavy baggage.")
        });

        // Direct Dhaka <-> Bandarban
        AddBidirectionalLeg(dhaka, bandarban, 320, 510, new()
        {
            (TransportMode.Bus, RouteTier.AcBus, "Saintmartin Travels / Shyamoli AC", 1400, 1800, 6, "Direct night coaches from Sayedabad & Arambagh to Bandarban town."),
            (TransportMode.Bus, RouteTier.NonAcBus, "S.Alam / Unique Service", 850, 950, 10, "Direct overnight buses reaching Bandarban early morning.")
        });

        // Chittagong <-> Rangamati
        AddBidirectionalLeg(chittagong, rangamati, 70, 120, new()
        {
            (TransportMode.Bus, RouteTier.NonAcBus, "Paharika Express / BRTC", 170, 230, 24, "From Muradpur or Oxygen bus stand to Rangamati terminal (2 hours)."),
            (TransportMode.Cng, RouteTier.ReservedVehicle, "Reserved Highway CNG / Car", 2000, 3000, 8, "Comfortable scenic mountain drive.")
        });

        // Direct Dhaka <-> Rangamati
        AddBidirectionalLeg(dhaka, rangamati, 310, 480, new()
        {
            (TransportMode.Bus, RouteTier.AcBus, "Hanif Enterprise / Shyamoli AC", 1200, 1600, 4, "Direct overnight services via Feni-Chittagong-Rangamati route."),
            (TransportMode.Bus, RouteTier.NonAcBus, "Unique / S.Alam", 800, 900, 8, "Daily scheduled departures from Dhaka.")
        });

        // Bandarban <-> Thanchi
        AddBidirectionalLeg(bandarban, thanchi, 82, 195, new()
        {
            (TransportMode.ChanderGari, RouteTier.LocalShared, "Chander Gari (Jeep / 4WD)", 250, 350, 6, "Local shared open jeep through dramatic mountain turns (3 - 3.5 hrs)."),
            (TransportMode.ChanderGari, RouteTier.ReservedVehicle, "Reserved 4x4 Chander Gari", 5500, 7500, 4, "Hired exclusively from Bandarban Jeep Station.")
        });

        // Rangamati <-> Sajek Valley
        AddBidirectionalLeg(rangamati, sajek, 125, 240, new()
        {
            (TransportMode.ChanderGari, RouteTier.ReservedVehicle, "Chander Gari (via Dighinala)", 7500, 10500, 10, "Traverses through scenic valleys and requires army convoy escort from Baghaichhari.")
        });

        // Remote Origins connecting to Dhaka
        AddBidirectionalLeg(sylhet, dhaka, 240, 330, new()
        {
            (TransportMode.Train, RouteTier.Snigdha, "Parabat / Upaban Express", 380, 800, 4, "Comfortable rail trip through tea estates."),
            (TransportMode.Bus, RouteTier.AcBus, "Green Line / Shohagh", 800, 1300, 20, "Departures from Subhanighat.")
        });

        AddBidirectionalLeg(sylhet, chittagong, 370, 480, new()
        {
            (TransportMode.Train, RouteTier.Snigdha, "Paharika / Udayan Express", 420, 890, 2, "Direct intercity train connecting Sylhet directly to Chittagong without passing Dhaka!"),
            (TransportMode.Bus, RouteTier.AcBus, "Ena / Shyamoli AC", 1100, 1500, 6, "Direct highway coach.")
        });

        AddBidirectionalLeg(rajshahi, dhaka, 250, 310, new()
        {
            (TransportMode.Train, RouteTier.Snigdha, "Silk City / Padma Express", 400, 850, 4, "Fast broad-gauge rail across Bangabandhu Jamuna Bridge."),
            (TransportMode.Bus, RouteTier.AcBus, "National Travels / Desh Travels", 900, 1400, 18, "Direct AC coach.")
        });

        AddBidirectionalLeg(khulna, dhaka, 220, 230, new()
        {
            (TransportMode.Bus, RouteTier.AcBus, "Shohagh / Tungipara Express", 850, 1400, 16, "High-speed travel over the Padma Bridge in under 4 hours."),
            (TransportMode.Train, RouteTier.Snigdha, "Sundarban Express", 450, 950, 2, "Rail route via Padma Bridge rail link.")
        });

        AddBidirectionalLeg(barisal, dhaka, 180, 180, new()
        {
            (TransportMode.Bus, RouteTier.AcBus, "Sakura Paribahan / Green Line", 700, 1100, 24, "Express travel via Padma Bridge in ~3 hours."),
            (TransportMode.Launch, RouteTier.ReservedVehicle, "Sundarban / Suravi / Parabat Luxury Cabin", 1200, 3500, 6, "Iconic overnight riverine triple-decker giant launch with private VIP cabins.")
        });

        await context.SaveChangesAsync();

        // 3. ATTRACTIONS
        var attractions = new List<Attraction>
        {
            // Bandarban
            new Attraction
            {
                LocationId = bandarban.Id,
                Name = "Nilgiri Hill Resort Viewpoint",
                BanglaName = "নীলগিরি",
                Description = "Perched at 2,200 feet above sea level, managed by the Bangladesh Army, floating literally above cloud banks.",
                BestTimeToVisit = "September to February (Monsoon & Winter for sea of clouds)",
                EntryFeeBdt = 100,
                Category = "Cloud Peaks",
                ImageUrl = "https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=800"
            },
            new Attraction
            {
                LocationId = bandarban.Id,
                Name = "Nafakhum Waterfalls",
                BanglaName = "নাফাখুম জলপ্রপাত",
                Description = "The Bengal Niagara — a thunderous cascade located along the Remakri canal accessible via engine boat from Thanchi.",
                BestTimeToVisit = "October to January",
                EntryFeeBdt = 0,
                Category = "Waterfalls",
                ImageUrl = "https://images.unsplash.com/photo-1432405972618-c60b0225b8f9?w=800"
            },
            new Attraction
            {
                LocationId = bandarban.Id,
                Name = "Boga Lake",
                BanglaName = "বগালেক",
                Description = "A mystical natural crater lake at 1,500 feet enveloped in Marma folklore, halfway to Keokradong peak.",
                BestTimeToVisit = "Winter months (November to March)",
                EntryFeeBdt = 50,
                Category = "Lakes & Treks",
                ImageUrl = "https://images.unsplash.com/photo-1470071459604-3b5ec3a7fe05?w=800"
            },

            // Cox's Bazar
            new Attraction
            {
                LocationId = coxsBazar.Id,
                Name = "Inani & Himchari Marine Drive Beach",
                BanglaName = "ইনানী ও হিমছড়ি সমুদ্রসৈকত",
                Description = "A breathtaking drive along the longest marine drive in the world featuring coral stones, sea spray, and hill cliffs.",
                BestTimeToVisit = "Year round (Sunset is magical)",
                EntryFeeBdt = 30,
                Category = "Beaches",
                ImageUrl = "https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=800"
            },
            new Attraction
            {
                LocationId = coxsBazar.Id,
                Name = "Saint Martin's Island Gateway (Teknaf / Cox)",
                BanglaName = "সেন্টমার্টিন দ্বীপ ট্রিপ",
                Description = "Bangladesh's only coral island boasting coconut groves, transparent azure water, and fresh crab barbecue.",
                BestTimeToVisit = "November to February (Ship season)",
                EntryFeeBdt = 0,
                Category = "Island",
                ImageUrl = "https://images.unsplash.com/photo-1519046904884-53103b34b206?w=800"
            },

            // Rangamati
            new Attraction
            {
                LocationId = rangamati.Id,
                Name = "Kaptai Lake Cruise & Hanging Bridge",
                BanglaName = "কাপ্তাই লেক ও ঝুলন্ত সেতু",
                Description = "The quintessential Rangamati experience — motorboat cruising across emerald blue hills and tribal villages.",
                BestTimeToVisit = "October to March",
                EntryFeeBdt = 50,
                Category = "Lakes",
                ImageUrl = "https://images.unsplash.com/photo-1501785888041-af3ef285b470?w=800"
            },
            new Attraction
            {
                LocationId = rangamati.Id,
                Name = "Shuvolong Waterfalls",
                BanglaName = "শুভলং ঝর্ণা",
                Description = "Massive cliffside waterfall accessible exclusively by boat from Rangamati reserve bazaar.",
                BestTimeToVisit = "July to October (Monsoon flow is majestic)",
                EntryFeeBdt = 30,
                Category = "Waterfalls",
                ImageUrl = "https://images.unsplash.com/photo-1432405972618-c60b0225b8f9?w=800"
            },

            // Chittagong
            new Attraction
            {
                LocationId = chittagong.Id,
                Name = "Patenga Sea Beach & Karnaphuli River Estuary",
                BanglaName = "পতেঙ্গা সমুদ্র সৈকত",
                Description = "Beloved coast where the river meets the sea, famous for spiced crispy mud-crab fry and fresh falooda.",
                BestTimeToVisit = "Late afternoons for breeze and night lights",
                EntryFeeBdt = 0,
                Category = "Coast",
                ImageUrl = "https://images.unsplash.com/photo-1596895111956-bf1cf0599ce5?w=800"
            },
            new Attraction
            {
                LocationId = chittagong.Id,
                Name = "Naval Beach & Bangabandhu Tunnel",
                BanglaName = "বঙ্গবন্ধু টানেল ও নেভাল",
                Description = "South Asia's first underwater river tunnel connecting Chittagong city to Anwara beach.",
                BestTimeToVisit = "Evening drive",
                EntryFeeBdt = 150,
                Category = "Landmark",
                ImageUrl = "https://images.unsplash.com/photo-1477959858617-67f30bc75b82?w=800"
            }
        };

        await context.Attractions.AddRangeAsync(attractions);

        // 4. ACCOMMODATIONS
        var accommodations = new List<Accommodation>
        {
            // Bandarban
            new Accommodation
            {
                LocationId = bandarban.Id,
                Name = "Sairu Hill Resort",
                BudgetLevel = BudgetLevel.Resort,
                ApproxPriceRange = "৳12,000 - ৳22,000 / night",
                Address = "Baro Mile, Bandarban-Chimbuk Road",
                ContactPhone = "+8801777771234",
                Rating = 4.8,
                HighlightFeature = "Infinity pool with 360-degree panoramic mountain cloud view"
            },
            new Accommodation
            {
                LocationId = bandarban.Id,
                Name = "Hill Crown Hotel & Resort",
                BudgetLevel = BudgetLevel.MidRange,
                ApproxPriceRange = "৳3,500 - ৳6,000 / night",
                Address = "Near Circuit House, Bandarban Sadar",
                ContactPhone = "+8801819001122",
                Rating = 4.2,
                HighlightFeature = "Central location with family mountain view suites"
            },
            new Accommodation
            {
                LocationId = bandarban.Id,
                Name = "Boga Lake Tribal Community Homestay",
                BudgetLevel = BudgetLevel.Budget,
                ApproxPriceRange = "৳500 - ৳1,000 / night",
                Address = "Boga Lake Village, Ruma",
                ContactPhone = "+8801822334455",
                Rating = 4.5,
                HighlightFeature = "Authentic Marma/Bawm wooden stilt cottage experience & campfire"
            },

            // Cox's Bazar
            new Accommodation
            {
                LocationId = coxsBazar.Id,
                Name = "Sayeman Beach Resort",
                BudgetLevel = BudgetLevel.Luxury,
                ApproxPriceRange = "৳8,000 - ৳18,000 / night",
                Address = "Marine Drive, Kolatoli Beach",
                ContactPhone = "+8801755699944",
                Rating = 4.7,
                HighlightFeature = "Direct oceanfront balcony & infinity pool facing Bay of Bengal"
            },
            new Accommodation
            {
                LocationId = coxsBazar.Id,
                Name = "Hotel Sea Crown",
                BudgetLevel = BudgetLevel.MidRange,
                ApproxPriceRange = "৳3,000 - ৳5,500 / night",
                Address = "Kolatoli Beach Front",
                ContactPhone = "+8801819876543",
                Rating = 4.3,
                HighlightFeature = "Private beach sitting lounge and open grill restaurant"
            },

            // Rangamati
            new Accommodation
            {
                LocationId = rangamati.Id,
                Name = "Aronnook Holiday Resort",
                BudgetLevel = BudgetLevel.Resort,
                ApproxPriceRange = "৳5,000 - ৳10,000 / night",
                Address = "Kaptai Lake Shore, Rangamati Sadar",
                ContactPhone = "+8801769300000",
                Rating = 4.6,
                HighlightFeature = "Overlooking Kaptai Lake with army leisure security & pedal boats"
            },
            new Accommodation
            {
                LocationId = rangamati.Id,
                Name = "Hotel Lake View",
                BudgetLevel = BudgetLevel.Budget,
                ApproxPriceRange = "৳1,500 - ৳2,800 / night",
                Address = "Reserve Bazaar, Rangamati",
                ContactPhone = "+8801815112233",
                Rating = 4.0,
                HighlightFeature = "Walkable to boat terminal for early morning Lake cruise"
            },

            // Sajek
            new Accommodation
            {
                LocationId = sajek.Id,
                Name = "Meghpunji Resort",
                BudgetLevel = BudgetLevel.EcoCottage,
                ApproxPriceRange = "৳4,500 - ৳8,000 / night",
                Address = "Ruilui Para, Sajek Valley",
                ContactPhone = "+8801883000000",
                Rating = 4.9,
                HighlightFeature = "Glass windows directly facing rolling white cloud valleys"
            }
        };

        await context.Accommodations.AddRangeAsync(accommodations);

        // 5. ADVISORIES & LOCAL TIPS
        var advisories = new List<DestinationAdvisory>
        {
            // Bandarban
            new DestinationAdvisory
            {
                LocationId = bandarban.Id,
                Category = "Mandatory Hill Tracts Permits",
                Title = "NID / Passport Copies Required for Army Checkposts",
                Content = "Carry at least 5-10 photocopies of your National ID card / Passport and 2 passport-size photographs. Every tourist must register at Army and Police checkposts at Ruma, Thanchi, and Chimbuk before proceeding.",
                IsMandatory = true
            },
            new DestinationAdvisory
            {
                LocationId = bandarban.Id,
                Category = "Guide Requirement",
                Title = "Registered Local Guide for Remakri / Nafakhum",
                Content = "Venturing beyond Thanchi into Remakri or Nafakhum waterfalls strictly requires hiring a certified guide from the Thanchi Guide Association (approx ৳1,500/day + food). Life jackets are compulsory when riding engine boats through Tindu boulders.",
                IsMandatory = true
            },
            new DestinationAdvisory
            {
                LocationId = bandarban.Id,
                Category = "Food Speciality",
                Title = "Tribal Cuisine: Bamboo Chicken & Mundi",
                Content = "Try indigenous Marma and Tripura delicacies. 'Bamboo Chicken' (chicken slow-cooked inside green bamboo culms over charcoal) and 'Mundi' (spicy homemade rice noodles served in hot aromatic broth) are available near Thanchi and Chimbuk hill shops.",
                IsMandatory = false
            },

            // Cox's Bazar
            new DestinationAdvisory
            {
                LocationId = coxsBazar.Id,
                Category = "Food Speciality",
                Title = "Fresh Seafood at Jhaubon & Poushee",
                Content = "Don't miss Ruponchanda fry, Koral fish bhuna, Loitta fry, and dried fish (Shutki) bharta at local landmark eateries like Poushee, Jhaubon, and Taranga near Kolatoli.",
                IsMandatory = false
            },
            new DestinationAdvisory
            {
                LocationId = coxsBazar.Id,
                Category = "Safety",
                Title = "Red Flag High Tide Warning",
                Content = "Observe beach safety lifeguard flags. Swimming during high tide or near rip currents (especially at Laboni and Sugandha points) is prohibited when red flags are raised.",
                IsMandatory = true
            },

            // Rangamati
            new DestinationAdvisory
            {
                LocationId = rangamati.Id,
                Category = "Local Transport",
                Title = "Renting Engine Boats at Reserve Bazaar",
                Content = "For visiting Shuvolong Waterfalls or Peda Ting Ting island restaurant, negotiate boat rates at Reserve Bazaar Ghat (typical day rental ranges ৳2,000 - ৳3,500 depending on boat capacity).",
                IsMandatory = false
            },
            new DestinationAdvisory
            {
                LocationId = rangamati.Id,
                Category = "Food Speciality",
                Title = "Peda Ting Ting & Kehang Bamboo Fish",
                Content = "Take a boat to lake restaurants like Peda Ting Ting or Tuk Tuk Eco Village to taste fresh Kaptai Lake carp roasted in bamboo tubes and native sticky rice with wild herb chutneys.",
                IsMandatory = false
            },

            // Sajek
            new DestinationAdvisory
            {
                LocationId = sajek.Id,
                Category = "Mandatory Escort",
                Title = "Army Convoy Timings from Dighinala",
                Content = "Vehicles cannot travel independently into Sajek Valley. You must join the Bangladesh Army Convoy from Dighinala checkpoint, which departs twice daily: Morning Convoy at 10:00 AM and Afternoon Convoy at 3:00 PM.",
                IsMandatory = true
            }
        };

        await context.DestinationAdvisories.AddRangeAsync(advisories);
        await context.SaveChangesAsync();
    }

    // ── Extended 50-Location Seed ─────────────────────────────────────────────
    public static async Task SeedExtendedLocationsAsync(TravelDbContext context)
    {
        // Guard: only run if fewer than 15 locations exist (base seed = 11)
        if (await context.Locations.CountAsync() >= 15)
            return;

        var extended = new List<Location>
        {
            // ── DHAKA DIVISION ─────────────────────────────────────────────
            new Location
            {
                Name = "Narayanganj",
                BanglaName = "নারায়ণগঞ্জ",
                Slug = "narayanganj",
                Type = LocationType.District,
                District = "Narayanganj",
                Division = "Dhaka",
                Latitude = 23.6238,
                Longitude = 90.4997,
                IsMajorHub = true,
                IsTouristDestination = true,
                Description = "Industrial port city on the Shitalakshya river, home to Sonargaon — the ancient capital of Bengal — and the haunting Panam City ruins.",
                HeroImageUrl = "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=1200"
            },
            new Location
            {
                Name = "Sonargaon",
                BanglaName = "সোনারগাঁ",
                Slug = "sonargaon",
                Type = LocationType.TouristSpot,
                District = "Narayanganj",
                Division = "Dhaka",
                Latitude = 23.6542,
                Longitude = 90.5960,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Medieval capital of Bengal featuring Panam City ghost town, Bangladesh Folk Art Museum, and labyrinthine terracotta merchant mansions.",
                HeroImageUrl = "https://images.unsplash.com/photo-1570168007204-dfb528c6958f?w=1200"
            },
            new Location
            {
                Name = "Manikganj",
                BanglaName = "মানিকগঞ্জ",
                Slug = "manikganj",
                Type = LocationType.District,
                District = "Manikganj",
                Division = "Dhaka",
                Latitude = 23.8647,
                Longitude = 90.0042,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Riverside district on the Jamuna plain, entry point for Padma river cruises and the historic Teota Zamindar Palace.",
                HeroImageUrl = "https://images.unsplash.com/photo-1506905925346-21bda4d32df4?w=1200"
            },
            new Location
            {
                Name = "Gazipur",
                BanglaName = "গাজীপুর",
                Slug = "gazipur",
                Type = LocationType.District,
                District = "Gazipur",
                Division = "Dhaka",
                Latitude = 23.9999,
                Longitude = 90.4203,
                IsMajorHub = true,
                IsTouristDestination = true,
                Description = "Gateway to Bhawal National Park, Nuhash Pallí eco resort, and the dense Sal forests north of Dhaka.",
                HeroImageUrl = "https://images.unsplash.com/photo-1441974231531-c6227db76b6e?w=1200"
            },
            new Location
            {
                Name = "Munshiganj",
                BanglaName = "মুন্সীগঞ্জ",
                Slug = "munshiganj",
                Type = LocationType.District,
                District = "Munshiganj",
                Division = "Dhaka",
                Latitude = 23.5422,
                Longitude = 90.5305,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Land of the Bikrampur kingdom, ferry gateway to the Padma Bridge, and home to the ancient Idrakpur Fort.",
                HeroImageUrl = "https://images.unsplash.com/photo-1473448912268-2022ce9509d8?w=1200"
            },

            // ── CHATTOGRAM DIVISION ──────────────────────────────────────────
            new Location
            {
                Name = "Feni",
                BanglaName = "ফেনী",
                Slug = "feni",
                Type = LocationType.District,
                District = "Feni",
                Division = "Chattogram",
                Latitude = 23.0224,
                Longitude = 91.3967,
                IsMajorHub = true,
                IsTouristDestination = false,
                Description = "Strategic transit corridor on the Dhaka–Chittagong highway; key interchange for travellers heading to Comilla, Noakhali, and Tripura border.",
                HeroImageUrl = "https://images.unsplash.com/photo-1504701954957-2010ec3bcec1?w=1200"
            },
            new Location
            {
                Name = "Comilla",
                BanglaName = "কুমিল্লা",
                Slug = "comilla",
                Type = LocationType.District,
                District = "Cumilla",
                Division = "Chattogram",
                Latitude = 23.4607,
                Longitude = 91.1809,
                IsMajorHub = true,
                IsTouristDestination = true,
                Description = "Ancient Mainamati Buddhist heritage city with hilltop monastery ruins and the country's finest Rashmалai sweets.",
                HeroImageUrl = "https://images.unsplash.com/photo-1555400038-63f5ba517a47?w=1200"
            },
            new Location
            {
                Name = "Noakhali",
                BanglaName = "নোয়াখালী",
                Slug = "noakhali",
                Type = LocationType.District,
                District = "Noakhali",
                Division = "Chattogram",
                Latitude = 22.8696,
                Longitude = 91.0993,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Coastal delta district and gateway to Nijhum Dwip mangrove island, home to spotted deer herds and nesting migratory birds.",
                HeroImageUrl = "https://images.unsplash.com/photo-1501854140801-50d01698950b?w=1200"
            },
            new Location
            {
                Name = "Nijhum Dwip",
                BanglaName = "নিঝুম দ্বীপ",
                Slug = "nijhum-dwip",
                Type = LocationType.TouristSpot,
                District = "Noakhali",
                Division = "Chattogram",
                Latitude = 22.0504,
                Longitude = 90.9738,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Remote island national park teeming with spotted deer, fishing boats, and pristine tidal mangrove channels — accessible only by boat from Hatiya.",
                HeroImageUrl = "https://images.unsplash.com/photo-1518020382113-a7e8fc38eac9?w=1200"
            },
            new Location
            {
                Name = "Khagrachhari",
                BanglaName = "খাগড়াছড়ি",
                Slug = "khagrachhari",
                Type = LocationType.TouristSpot,
                District = "Khagrachhari",
                Division = "Chattogram",
                Latitude = 23.1193,
                Longitude = 91.9847,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Hill district home to Aluutila Mystery Cave, Richhang Waterfall, and the scenic road to Sajek Valley through Dighinala.",
                HeroImageUrl = "https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?w=1200"
            },
            new Location
            {
                Name = "Teknaf",
                BanglaName = "টেকনাফ",
                Slug = "teknaf",
                Type = LocationType.TouristSpot,
                District = "Cox's Bazar",
                Division = "Chattogram",
                Latitude = 20.8642,
                Longitude = 92.3011,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Southernmost tip of Bangladesh and ferry port for Saint Martin's Island; bordered by Myanmar's Naf River with unique tidal mangroves.",
                HeroImageUrl = "https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=1200"
            },
            new Location
            {
                Name = "Saint Martin's Island",
                BanglaName = "সেন্ট মার্টিন দ্বীপ",
                Slug = "saint-martins-island",
                Type = LocationType.TouristSpot,
                District = "Cox's Bazar",
                Division = "Chattogram",
                Latitude = 20.6273,
                Longitude = 92.3239,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Bangladesh's only coral island — coconut-fringed shores, crystal-clear turquoise water, fresh crab BBQ, and sea-turtle nesting beaches.",
                HeroImageUrl = "https://images.unsplash.com/photo-1519046904884-53103b34b206?w=1200"
            },

            // ── SYLHET DIVISION ──────────────────────────────────────────────
            new Location
            {
                Name = "Sreemangal",
                BanglaName = "শ্রীমঙ্গল",
                Slug = "sreemangal",
                Type = LocationType.TouristSpot,
                District = "Moulvibazar",
                Division = "Sylhet",
                Latitude = 24.3065,
                Longitude = 91.7284,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Tea capital of Bangladesh — rolling emerald tea gardens, seven-layer tea, Lawachara rainforest trek, and migratory bird hotspots.",
                HeroImageUrl = "https://images.unsplash.com/photo-1578632767115-351597cf2477?w=1200"
            },
            new Location
            {
                Name = "Moulvibazar",
                BanglaName = "মৌলভীবাজার",
                Slug = "moulvibazar",
                Type = LocationType.District,
                District = "Moulvibazar",
                Division = "Sylhet",
                Latitude = 24.4829,
                Longitude = 91.7774,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Heartland of Sylhet tea belt bordering Tripura, gateway to Hakaluki Haor wetlands and the Baikka Beel bird sanctuary.",
                HeroImageUrl = "https://images.unsplash.com/photo-1441974231531-c6227db76b6e?w=1200"
            },
            new Location
            {
                Name = "Sunamganj",
                BanglaName = "সুনামগঞ্জ",
                Slug = "sunamganj",
                Type = LocationType.TouristSpot,
                District = "Sunamganj",
                Division = "Sylhet",
                Latitude = 25.0658,
                Longitude = 91.3950,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Gateway to Tanguar Haor — the UNESCO Ramsar wetland — and the mystical Shimul forest of Yadukata along the Meghalaya border.",
                HeroImageUrl = "https://images.unsplash.com/photo-1501785888041-af3ef285b470?w=1200"
            },
            new Location
            {
                Name = "Tanguar Haor",
                BanglaName = "টাঙ্গুয়ার হাওর",
                Slug = "tanguar-haor",
                Type = LocationType.TouristSpot,
                District = "Sunamganj",
                Division = "Sylhet",
                Latitude = 25.1597,
                Longitude = 91.1145,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "UNESCO Ramsar wetland spanning 100 sq km — houseboat nights under the Milky Way, migratory ducks in winter, and crystal-clear Meghalaya hill runoff.",
                HeroImageUrl = "https://images.unsplash.com/photo-1506905925346-21bda4d32df4?w=1200"
            },
            new Location
            {
                Name = "Jaflong",
                BanglaName = "জাফলং",
                Slug = "jaflong",
                Type = LocationType.TouristSpot,
                District = "Sylhet",
                Division = "Sylhet",
                Latitude = 25.1556,
                Longitude = 92.0360,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Breathtaking river pebble delta at the foot of the Khasi Hills where the Piyain River flows in from Meghalaya with turquoise glacial water.",
                HeroImageUrl = "https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=1200"
            },
            new Location
            {
                Name = "Ratargul Swamp Forest",
                BanglaName = "রাতারগুল জলারবন",
                Slug = "ratargul",
                Type = LocationType.TouristSpot,
                District = "Sylhet",
                Division = "Sylhet",
                Latitude = 25.0124,
                Longitude = 91.8536,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Bangladesh's Amazon — a freshwater swamp forest flooded 8 months a year, navigated exclusively by wooden rowboat through submerged Hijal and Koroch trees.",
                HeroImageUrl = "https://images.unsplash.com/photo-1518020382113-a7e8fc38eac9?w=1200"
            },
            new Location
            {
                Name = "Habiganj",
                BanglaName = "হবিগঞ্জ",
                Slug = "habiganj",
                Type = LocationType.District,
                District = "Habiganj",
                Division = "Sylhet",
                Latitude = 24.3742,
                Longitude = 91.4145,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Oil-gas industrial district on the edge of Sylhet tea country, gateway to Satchari National Park and its rare Hoolock gibbons.",
                HeroImageUrl = "https://images.unsplash.com/photo-1441974231531-c6227db76b6e?w=1200"
            },

            // ── RAJSHAHI DIVISION ────────────────────────────────────────────
            new Location
            {
                Name = "Bogura",
                BanglaName = "বগুড়া",
                Slug = "bogura",
                Type = LocationType.District,
                District = "Bogura",
                Division = "Rajshahi",
                Latitude = 24.8465,
                Longitude = 89.3773,
                IsMajorHub = true,
                IsTouristDestination = true,
                Description = "Gateway to Mahasthangarh — the oldest archaeological site in Bangladesh, a 2,500-year-old fortified city on the Karatoya riverbank.",
                HeroImageUrl = "https://images.unsplash.com/photo-1555400038-63f5ba517a47?w=1200"
            },
            new Location
            {
                Name = "Mahasthangarh",
                BanglaName = "মহাস্থানগড়",
                Slug = "mahasthangarh",
                Type = LocationType.TouristSpot,
                District = "Bogura",
                Division = "Rajshahi",
                Latitude = 24.9702,
                Longitude = 89.3392,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Oldest archaeological city in Bangladesh dating to 300 BCE — massive earthen ramparts, Mauryan artifacts, and the sacred Mazaar of Shah Sultan Balkhi.",
                HeroImageUrl = "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=1200"
            },
            new Location
            {
                Name = "Natore",
                BanglaName = "নাটোর",
                Slug = "natore",
                Type = LocationType.District,
                District = "Natore",
                Division = "Rajshahi",
                Latitude = 24.4103,
                Longitude = 88.9876,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Home of the legendary Rani Bhabani palace complex and gateway to the Baraichara Wetlands — winter haven for migratory waterfowl.",
                HeroImageUrl = "https://images.unsplash.com/photo-1570168007204-dfb528c6958f?w=1200"
            },
            new Location
            {
                Name = "Chapai Nawabganj",
                BanglaName = "চাঁপাইনবাবগঞ্জ",
                Slug = "chapai-nawabganj",
                Type = LocationType.District,
                District = "Chapai Nawabganj",
                Division = "Rajshahi",
                Latitude = 24.5917,
                Longitude = 88.2743,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Mango capital of Bangladesh — vast Fazli and Langra orchards, Sona Mosque ruins, and the Ganges (Padma) border with India.",
                HeroImageUrl = "https://images.unsplash.com/photo-1513836279014-a89f7a76ae86?w=1200"
            },
            new Location
            {
                Name = "Pabna",
                BanglaName = "পাবনা",
                Slug = "pabna",
                Type = LocationType.District,
                District = "Pabna",
                Division = "Rajshahi",
                Latitude = 24.0064,
                Longitude = 89.2372,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Textile and sugar town on the Padma plain; home to the historic Pabna Zamindar Palace and the country's longest hydraulic sluice gate at Bera.",
                HeroImageUrl = "https://images.unsplash.com/photo-1504701954957-2010ec3bcec1?w=1200"
            },

            // ── RANGPUR DIVISION ─────────────────────────────────────────────
            new Location
            {
                Name = "Rangpur",
                BanglaName = "রংপুর",
                Slug = "rangpur",
                Type = LocationType.District,
                District = "Rangpur",
                Division = "Rangpur",
                Latitude = 25.7439,
                Longitude = 89.2752,
                IsMajorHub = true,
                IsTouristDestination = false,
                Description = "Northern divisional capital known for its textile weaving and gateway to Kantajew Temple and the Teesta barrage wetlands.",
                HeroImageUrl = "https://images.unsplash.com/photo-1518684079-3c830dcef090?w=1200"
            },
            new Location
            {
                Name = "Dinajpur",
                BanglaName = "দিনাজপুর",
                Slug = "dinajpur",
                Type = LocationType.District,
                District = "Dinajpur",
                Division = "Rangpur",
                Latitude = 25.6279,
                Longitude = 88.6330,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Ancient temple city of the north famed for Kantajew terracotta temple, Nayabad Mosque, and the finest Kataribhog aromatic rice in the country.",
                HeroImageUrl = "https://images.unsplash.com/photo-1555400038-63f5ba517a47?w=1200"
            },
            new Location
            {
                Name = "Kantajew Temple",
                BanglaName = "কান্তজীউ মন্দির",
                Slug = "kantajew-temple",
                Type = LocationType.TouristSpot,
                District = "Dinajpur",
                Division = "Rangpur",
                Latitude = 25.8491,
                Longitude = 88.6131,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "18th-century nine-spired terracotta Hindu temple with over 15,000 intricate mythological panels — the finest terracotta architecture in Bangladesh.",
                HeroImageUrl = "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=1200"
            },
            new Location
            {
                Name = "Lalmonirhat",
                BanglaName = "লালমনিরহাট",
                Slug = "lalmonirhat",
                Type = LocationType.District,
                District = "Lalmonirhat",
                Division = "Rangpur",
                Latitude = 25.9923,
                Longitude = 89.2846,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Teesta river district containing Burimari land port with India, and the unique 'Chitmahals' — historic enclaves exchanged between Bangladesh and India in 2015.",
                HeroImageUrl = "https://images.unsplash.com/photo-1501785888041-af3ef285b470?w=1200"
            },
            new Location
            {
                Name = "Panchagarh",
                BanglaName = "পঞ্চগড়",
                Slug = "panchagarh",
                Type = LocationType.TouristSpot,
                District = "Panchagarh",
                Division = "Rangpur",
                Latitude = 26.3408,
                Longitude = 88.5558,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Northernmost district of Bangladesh — on clear winter mornings Mount Kanchenjunga is visible from here; also home to the Tea Research Station and zero-point obelisk.",
                HeroImageUrl = "https://images.unsplash.com/photo-1486870591958-9b9d0d1dda99?w=1200"
            },

            // ── MYMENSINGH DIVISION ──────────────────────────────────────────
            new Location
            {
                Name = "Mymensingh",
                BanglaName = "ময়মনসিংহ",
                Slug = "mymensingh",
                Type = LocationType.District,
                District = "Mymensingh",
                Division = "Mymensingh",
                Latitude = 24.7471,
                Longitude = 90.4203,
                IsMajorHub = true,
                IsTouristDestination = true,
                Description = "Brahmaputra riverside city featuring the magnificent Alexander Castle, Bangladesh Agricultural University, and access to Boro Haor wetlands.",
                HeroImageUrl = "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=1200"
            },
            new Location
            {
                Name = "Netrokona",
                BanglaName = "নেত্রকোনা",
                Slug = "netrokona",
                Type = LocationType.TouristSpot,
                District = "Netrokona",
                Division = "Mymensingh",
                Latitude = 24.8703,
                Longitude = 90.7270,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Haor district bordering Meghalaya — gateway to Birishiri white clay hills, Someshwari riverside, and the colourful Garo tribal culture.",
                HeroImageUrl = "https://images.unsplash.com/photo-1506905925346-21bda4d32df4?w=1200"
            },
            new Location
            {
                Name = "Birishiri",
                BanglaName = "বিরিশিরি",
                Slug = "birishiri",
                Type = LocationType.TouristSpot,
                District = "Netrokona",
                Division = "Mymensingh",
                Latitude = 24.9847,
                Longitude = 90.7741,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Turquoise Someshwari River flowing over white clay and blue clay hills — a photographer's paradise near the Meghalaya border with Garo heritage villages.",
                HeroImageUrl = "https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=1200"
            },
            new Location
            {
                Name = "Sherpur",
                BanglaName = "শেরপুর",
                Slug = "sherpur",
                Type = LocationType.District,
                District = "Sherpur",
                Division = "Mymensingh",
                Latitude = 25.0186,
                Longitude = 90.0152,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Gateway to Garo Hills buffer zone, Gazni Eco Park, and the unique Madhutila Wildlife Sanctuary bordering Meghalaya.",
                HeroImageUrl = "https://images.unsplash.com/photo-1441974231531-c6227db76b6e?w=1200"
            },

            // ── KHULNA DIVISION ──────────────────────────────────────────────
            new Location
            {
                Name = "Sundarbans",
                BanglaName = "সুন্দরবন",
                Slug = "sundarbans",
                Type = LocationType.TouristSpot,
                District = "Khulna",
                Division = "Khulna",
                Latitude = 21.9497,
                Longitude = 89.1833,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "UNESCO World Heritage mangrove forest — largest in the world, home to the Royal Bengal Tiger, Irrawaddy dolphins, and ancient tidal river channels.",
                HeroImageUrl = "https://images.unsplash.com/photo-1513836279014-a89f7a76ae86?w=1200"
            },
            new Location
            {
                Name = "Jessore",
                BanglaName = "যশোর",
                Slug = "jessore",
                Type = LocationType.District,
                District = "Jessore",
                Division = "Khulna",
                Latitude = 23.1664,
                Longitude = 89.2081,
                IsMajorHub = true,
                IsTouristDestination = true,
                Description = "Flower district of Bangladesh — wholesale gerbera and gladiolus fields; also has the Jessore Cantonment Museum and the historic Shesh Nag temple.",
                HeroImageUrl = "https://images.unsplash.com/photo-1501854140801-50d01698950b?w=1200"
            },
            new Location
            {
                Name = "Satkhira",
                BanglaName = "সাতক্ষীরা",
                Slug = "satkhira",
                Type = LocationType.TouristSpot,
                District = "Satkhira",
                Division = "Khulna",
                Latitude = 22.7185,
                Longitude = 89.0705,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Sundarban western gate district featuring Shyamnagar launch terminal, Hijla forest watch-tower camps, and Munshiganj handloom weavers.",
                HeroImageUrl = "https://images.unsplash.com/photo-1513836279014-a89f7a76ae86?w=1200"
            },
            new Location
            {
                Name = "Kushtia",
                BanglaName = "কুষ্টিয়া",
                Slug = "kushtia",
                Type = LocationType.District,
                District = "Kushtia",
                Division = "Khulna",
                Latitude = 23.9014,
                Longitude = 89.1191,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Spiritual heartland of the Baul tradition — birthplace of Lalon Shah, Tagore's Shilaidaha Kuthibari estate on the Padma, and country's finest Zari weaving.",
                HeroImageUrl = "https://images.unsplash.com/photo-1570168007204-dfb528c6958f?w=1200"
            },
            new Location
            {
                Name = "Shilaidaha Kuthibari",
                BanglaName = "শিলাইদহ কুঠিবাড়ি",
                Slug = "shilaidaha-kuthibari",
                Type = LocationType.TouristSpot,
                District = "Kushtia",
                Division = "Khulna",
                Latitude = 24.0432,
                Longitude = 89.0684,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Rabindranath Tagore's riverside estate where he composed much of Gitanjali — preserved with his original furniture, boats, and gardens on the Padma bank.",
                HeroImageUrl = "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=1200"
            },
            new Location
            {
                Name = "Mongla",
                BanglaName = "মোংলা",
                Slug = "mongla",
                Type = LocationType.TouristSpot,
                District = "Bagerhat",
                Division = "Khulna",
                Latitude = 22.4836,
                Longitude = 89.5895,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Primary launch port for Sundarban tiger territory forest safaris, Karamjal breeding centre for Bengal tigers and saltwater crocodiles.",
                HeroImageUrl = "https://images.unsplash.com/photo-1501785888041-af3ef285b470?w=1200"
            },
            new Location
            {
                Name = "Bagerhat",
                BanglaName = "বাগেরহাট",
                Slug = "bagerhat",
                Type = LocationType.TouristSpot,
                District = "Bagerhat",
                Division = "Khulna",
                Latitude = 22.6602,
                Longitude = 89.7895,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "UNESCO World Heritage site — Mosque City of Bagerhat featuring the magnificent 60-Dome Mosque (Shait Gumbad) built by Khan Jahan Ali in the 15th century.",
                HeroImageUrl = "https://images.unsplash.com/photo-1555400038-63f5ba517a47?w=1200"
            },

            // ── BARISHAL DIVISION ────────────────────────────────────────────
            new Location
            {
                Name = "Bhola",
                BanglaName = "ভোলা",
                Slug = "bhola",
                Type = LocationType.District,
                District = "Bhola",
                Division = "Barishal",
                Latitude = 22.6908,
                Longitude = 90.6588,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Bangladesh's largest river island district in the Bay of Bengal delta — famous for Char Kukri Mukri forest island with its unique spotted deer population.",
                HeroImageUrl = "https://images.unsplash.com/photo-1518020382113-a7e8fc38eac9?w=1200"
            },
            new Location
            {
                Name = "Patuakhali",
                BanglaName = "পটুয়াখালী",
                Slug = "patuakhali",
                Type = LocationType.District,
                District = "Patuakhali",
                Division = "Barishal",
                Latitude = 22.3596,
                Longitude = 90.3296,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Coastal district gateway to Kuakata Beach — the only sea beach in Bangladesh where you can see both sunrise and sunset from the same point.",
                HeroImageUrl = "https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=1200"
            },
            new Location
            {
                Name = "Kuakata",
                BanglaName = "কুয়াকাটা",
                Slug = "kuakata",
                Type = LocationType.TouristSpot,
                District = "Patuakhali",
                Division = "Barishal",
                Latitude = 21.8298,
                Longitude = 90.1200,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "The Daughter of the Sea — a 30-km beach where both sunrise AND sunset are visible from the same shore, with Rakhain Buddhist fisher villages nearby.",
                HeroImageUrl = "https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=1200"
            },
            new Location
            {
                Name = "Pirojpur",
                BanglaName = "পিরোজপুর",
                Slug = "pirojpur",
                Type = LocationType.District,
                District = "Pirojpur",
                Division = "Barishal",
                Latitude = 22.5841,
                Longitude = 89.9741,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Floating guava market destination — Swarupkathi's thousands of guava orchards are harvested by boat in one of the most unique river markets in South Asia.",
                HeroImageUrl = "https://images.unsplash.com/photo-1473448912268-2022ce9509d8?w=1200"
            },
            new Location
            {
                Name = "Jhalokati",
                BanglaName = "ঝালকাঠি",
                Slug = "jhalokati",
                Type = LocationType.District,
                District = "Jhalokati",
                Division = "Barishal",
                Latitude = 22.6407,
                Longitude = 90.1988,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Tiny canal-crisscrossed district famed for the Atghora canal boat journey — a floating village surrounded by sapota (sofeda) and betel nut farms.",
                HeroImageUrl = "https://images.unsplash.com/photo-1473448912268-2022ce9509d8?w=1200"
            },

            // ── ADDITIONAL TOURIST SPOTS ─────────────────────────────────────
            new Location
            {
                Name = "Srimangal Tea Gardens",
                BanglaName = "শ্রীমঙ্গল চা বাগান",
                Slug = "srimangal-tea-gardens",
                Type = LocationType.TouristSpot,
                District = "Moulvibazar",
                Division = "Sylhet",
                Latitude = 24.3023,
                Longitude = 91.7264,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Finlay and Duncan tea estates stretching over 3,000 acres of rolling emerald hills — the source of Bangladesh's world-famous premium Orthodox tea.",
                HeroImageUrl = "https://images.unsplash.com/photo-1578632767115-351597cf2477?w=1200"
            },
            new Location
            {
                Name = "Jaipurhat",
                BanglaName = "জয়পুরহাট",
                Slug = "jaipurhat",
                Type = LocationType.District,
                District = "Joypurhat",
                Division = "Rajshahi",
                Latitude = 25.1031,
                Longitude = 89.0229,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Small sugarcane district with the ancient Pundra terracotta site and the unique Nandail Dighi royal reservoir — a quiet heritage gem in the north.",
                HeroImageUrl = "https://images.unsplash.com/photo-1504701954957-2010ec3bcec1?w=1200"
            },
            new Location
            {
                Name = "Naogaon",
                BanglaName = "নওগাঁ",
                Slug = "naogaon",
                Type = LocationType.TouristSpot,
                District = "Naogaon",
                Division = "Rajshahi",
                Latitude = 24.7936,
                Longitude = 88.9316,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Home of Paharpur Buddhist Monastery (Somapura Mahavihara) — the largest Buddhist monastery south of the Himalayas, a UNESCO World Heritage Site.",
                HeroImageUrl = "https://images.unsplash.com/photo-1555400038-63f5ba517a47?w=1200"
            },
            new Location
            {
                Name = "Paharpur Buddhist Monastery",
                BanglaName = "পাহাড়পুর বৌদ্ধ বিহার",
                Slug = "paharpur",
                Type = LocationType.TouristSpot,
                District = "Naogaon",
                Division = "Rajshahi",
                Latitude = 25.0303,
                Longitude = 88.9783,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "UNESCO World Heritage ruins of the 8th-century Somapura Mahavihara — largest Buddhist monastery south of the Himalayas with over 177 meditation cells.",
                HeroImageUrl = "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=1200"
            },
            new Location
            {
                Name = "Sirajganj",
                BanglaName = "সিরাজগঞ্জ",
                Slug = "sirajganj",
                Type = LocationType.District,
                District = "Sirajganj",
                Division = "Rajshahi",
                Latitude = 24.4533,
                Longitude = 89.7058,
                IsMajorHub = true,
                IsTouristDestination = false,
                Description = "Jamuna riverside district at the base of the Bangabandhu Bridge; transit hub for western route trains and major weaving town for Tant cotton sarees.",
                HeroImageUrl = "https://images.unsplash.com/photo-1506905925346-21bda4d32df4?w=1200"
            },
            new Location
            {
                Name = "Tangail",
                BanglaName = "টাঙ্গাইল",
                Slug = "tangail",
                Type = LocationType.District,
                District = "Tangail",
                Division = "Dhaka",
                Latitude = 24.2513,
                Longitude = 89.9167,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Saree weaving capital at the confluence of three rivers; home to Atia Mosque, Dhanbari Nawab Palace, and the scenic Madhupur Sal forest.",
                HeroImageUrl = "https://images.unsplash.com/photo-1570168007204-dfb528c6958f?w=1200"
            },
            new Location
            {
                Name = "Narsingdi",
                BanglaName = "নরসিংদী",
                Slug = "narsingdi",
                Type = LocationType.District,
                District = "Narsingdi",
                Division = "Dhaka",
                Latitude = 23.9215,
                Longitude = 90.7148,
                IsMajorHub = false,
                IsTouristDestination = false,
                Description = "Textile and fruit district east of Dhaka along the Meghna; a key transit corridor for travellers heading toward Comilla and Chittagong by road.",
                HeroImageUrl = "https://images.unsplash.com/photo-1504701954957-2010ec3bcec1?w=1200"
            },
            new Location
            {
                Name = "Kishoreganj",
                BanglaName = "কিশোরগঞ্জ",
                Slug = "kishoreganj",
                Type = LocationType.TouristSpot,
                District = "Kishoreganj",
                Division = "Dhaka",
                Latitude = 24.4443,
                Longitude = 90.7759,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Haor heartland district featuring the vast Hakaluki and Nikli Haor wetlands — magical boat journeys through submerged paddy plains in the monsoon season.",
                HeroImageUrl = "https://images.unsplash.com/photo-1506905925346-21bda4d32df4?w=1200"
            },
            new Location
            {
                Name = "Chandpur",
                BanglaName = "চাঁদপুর",
                Slug = "chandpur",
                Type = LocationType.District,
                District = "Chandpur",
                Division = "Chattogram",
                Latitude = 23.2333,
                Longitude = 90.6517,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Hilsa fish capital of Bangladesh at the confluence of Padma, Meghna, and Dakatia rivers — overnight launches from Dhaka arrive at dawn with stunning river views.",
                HeroImageUrl = "https://images.unsplash.com/photo-1473448912268-2022ce9509d8?w=1200"
            },
            new Location
            {
                Name = "Netrakona Birishiri",
                BanglaName = "বিরিশিরি নেত্রকোনা",
                Slug = "birishiri-someshwari",
                Type = LocationType.TouristSpot,
                District = "Netrokona",
                Division = "Mymensingh",
                Latitude = 25.0001,
                Longitude = 90.7602,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Someshwari River with its brilliant turquoise water running through white-clay hills, Garo Christian community villages, and seasonal guava-river markets.",
                HeroImageUrl = "https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=1200"
            },
            new Location
            {
                Name = "Kurigram",
                BanglaName = "কুড়িগ্রাম",
                Slug = "kurigram",
                Type = LocationType.District,
                District = "Kurigram",
                Division = "Rangpur",
                Latitude = 25.8063,
                Longitude = 89.6360,
                IsMajorHub = false,
                IsTouristDestination = false,
                Description = "Remote Teesta-Brahmaputra char district in the far north; char island communities with unique lifestyle accessible by country boat across braided river channels.",
                HeroImageUrl = "https://images.unsplash.com/photo-1501785888041-af3ef285b470?w=1200"
            },
            new Location
            {
                Name = "Gaibandha",
                BanglaName = "গাইবান্ধা",
                Slug = "gaibandha",
                Type = LocationType.District,
                District = "Gaibandha",
                Division = "Rangpur",
                Latitude = 25.3286,
                Longitude = 89.5286,
                IsMajorHub = false,
                IsTouristDestination = false,
                Description = "Jamuna–Teesta char district with unique Santali tribal communities; transit point for travellers going to Bogura and Rangpur from Dhaka.",
                HeroImageUrl = "https://images.unsplash.com/photo-1504701954957-2010ec3bcec1?w=1200"
            },
            new Location
            {
                Name = "Meherpur",
                BanglaName = "মেহেরপুর",
                Slug = "meherpur",
                Type = LocationType.TouristSpot,
                District = "Meherpur",
                Division = "Khulna",
                Latitude = 23.7621,
                Longitude = 88.6318,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Historic Mujibnagar — the provisional capital of independent Bangladesh in 1971; the Mujibnagar Complex with original mango grove and memorial are must visits.",
                HeroImageUrl = "https://images.unsplash.com/photo-1555400038-63f5ba517a47?w=1200"
            },
            new Location
            {
                Name = "Chuadanga",
                BanglaName = "চুয়াডাঙ্গা",
                Slug = "chuadanga",
                Type = LocationType.District,
                District = "Chuadanga",
                Division = "Khulna",
                Latitude = 23.6401,
                Longitude = 88.8413,
                IsMajorHub = false,
                IsTouristDestination = false,
                Description = "India–Bangladesh border district along the Mathabhanga river with the Darshana land port — one of the busiest rail crossings between the two countries.",
                HeroImageUrl = "https://images.unsplash.com/photo-1504701954957-2010ec3bcec1?w=1200"
            },
            new Location
            {
                Name = "Magura",
                BanglaName = "মাগুরা",
                Slug = "magura",
                Type = LocationType.District,
                District = "Magura",
                Division = "Khulna",
                Latitude = 23.4873,
                Longitude = 89.4193,
                IsMajorHub = false,
                IsTouristDestination = false,
                Description = "Small agro-processing district on the Nabaganga river; transit passage for travellers heading south from Dhaka towards the Sundarban coast.",
                HeroImageUrl = "https://images.unsplash.com/photo-1513836279014-a89f7a76ae86?w=1200"
            },
            new Location
            {
                Name = "Narail",
                BanglaName = "নড়াইল",
                Slug = "narail",
                Type = LocationType.District,
                District = "Narail",
                Division = "Khulna",
                Latitude = 23.1724,
                Longitude = 89.5122,
                IsMajorHub = false,
                IsTouristDestination = true,
                Description = "Birthplace of legendary artist S.M. Sultan; the Naldanga Zamindar Palace and Chitra River bank are quiet heritage getaways from Khulna city.",
                HeroImageUrl = "https://images.unsplash.com/photo-1570168007204-dfb528c6958f?w=1200"
            },
        };

        // Fix: Habiganj longitude was stored as string above, correct it
        var habiganj = extended.FirstOrDefault(l => l.Slug == "habiganj");
        if (habiganj != null) habiganj.Longitude = 91.4145;

        await context.Locations.AddRangeAsync(extended);
        await context.SaveChangesAsync();
    }
}
